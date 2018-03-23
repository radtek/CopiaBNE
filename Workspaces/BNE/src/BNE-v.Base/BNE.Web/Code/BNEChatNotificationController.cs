using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Web;
using BNE.BLL;
using BNE.Chat.Core;
using BNE.Chat.Core.Notification;
using BNE.Chat.Helper;

namespace BNE.Web.Code
{
    public sealed class BNEChatNotificationController : NotificationControllerBase, IDisposable
    {
        public class CountEventArgs : EventArgs
        {
            public CountEventArgs(int count)
            {
                this._count = count;
            }
            private readonly int _count;

            public int Count
            {
                get { return _count; }
            }
        }

        private event EventHandler<CountEventArgs> CountStore;
        private void OnCountStore(CountEventArgs e)
        {
            EventHandler<CountEventArgs> handler = CountStore;
            if (handler != null) handler(this, e);
        }

        public static readonly SetValueOrDefaultFact<HardConfig<int>, int> NotificationUpdatePoolTime =
       new HardConfig<int>("chat_notificacao_update_database_em_minutos", 10).Wrap(a => a.Value);

        public static readonly SetValueOrDefaultFact<HardConfig<int>, int> NotificationUpdateMaxQuantity =
       new HardConfig<int>("chat_notificacao_quantidade_para_update_database", 50).Wrap(a => a.Value);

        private int _totalCountAccumulated;
        private readonly IDisposable _disposableSubscription;

        #region [ Constructor ]
        public BNEChatNotificationController(NotificationStore notificationStore)
            : base(notificationStore)
        {
            var subs = Observable.FromEventPattern<CountEventArgs>(add => this.CountStore += add, rem => this.CountStore -= rem).Publish().RefCount();

            this._disposableSubscription = subs.Select(a => Unit.Default)
                .Buffer(() =>
                    Observable.Amb(Observable.Timer(TimeSpan.FromMinutes(NotificationUpdatePoolTime.Value)).Select(a => Unit.Default).Take(1),
                    subs.Where(a => a.EventArgs.Count >= NotificationUpdateMaxQuantity.Value).Select(a => Unit.Default).Take(1)).Take(1)
                )
                .Where(a => a.Count > 0)
                .Do(a => Interlocked.Exchange(ref _totalCountAccumulated, 0))
                .ObserveOn(Scheduler.Default)
                .Synchronize()
                .Subscribe(a => ConsumeOfflineNotificationItems());
        }

        private void ConsumeOfflineNotificationItems()
        {
            BackgroundTaskManager.Run(() =>
                {
                    try
                    {
                        var offLine = NotificationStore.GetAllOffline().ToArray();

                        if (offLine.Length <= 0)
                            return;

                        var toInsertOrUpdate = new List<NotificationGroup>();
                        foreach (var item in offLine)
                        {
                            using (NotificationStore.ConcurrencyManager.GetToken(item.KeyGroupId))
                            {
                                if (item.Expired)
                                    continue;

                                NotificationGroup removed;
                                if (NotificationStore.TryRemoveFromOfflineCache(item.KeyGroupId, out removed))
                                {
                                    toInsertOrUpdate.Add(removed);
                                }
                            }
                        }

                        var modelCollection =
                            toInsertOrUpdate.SelectMany(
                                group =>
                                    group.GetItemsToPersistence().Select(b => ConvertToDb(group.KeyGroupId, b.Key, b.Value)));

                        ConversasAtivas.AtualizarOuInserirVarios(modelCollection.ToArray());
                    }
                    catch (Exception ex)
                    {
                        // todo log
                    }
                });
        }
        #endregion

        #region [ Public Methods ]
        public override void NewOnlineNotificationPending(EventPattern<NotificationEventArgs> obj)
        {
            var group = NotificationStore.GetOrCreateOnlineGroup(obj.EventArgs.OwnerId);
            group.Item1.AddPending(obj.EventArgs.PartyWith, false);
        }

        public override void NewOnlineNotificationRead(EventPattern<NotificationEventArgs> obj)
        {
            var group = NotificationStore.GetOrCreateOnlineGroup(obj.EventArgs.OwnerId);
            group.Item1.AddRead(obj.EventArgs.PartyWith);
        }

        public override void NotificationPendingNotHandled(EventPattern<NotificationEventArgs> obj)
        {
            OnlineNotificationGroup onlineGroup;
            if (NotificationStore.GetOnlineGroup(obj.EventArgs.OwnerId, out onlineGroup))
            {
                onlineGroup.AddPending(obj.EventArgs.PartyWith, false);
                return;
            }

            var group = NotificationStore.GetOrCreateOffLineGroup(obj.EventArgs.OwnerId);
            var res = group.Item1.AddPending(obj.EventArgs.PartyWith, false);
            if (res)
                Interlocked.Increment(ref _totalCountAccumulated);
        }

        public override void NotificationReadNotHandled(EventPattern<NotificationEventArgs> obj)
        {
            OnlineNotificationGroup onlineGroup;
            if (NotificationStore.GetOnlineGroup(obj.EventArgs.OwnerId, out onlineGroup))
            {
                onlineGroup.AddRead(obj.EventArgs.PartyWith);
                return;
            }

            var group = NotificationStore.GetOrCreateOffLineGroup(obj.EventArgs.OwnerId);
            var res = group.Item1.AddRead(obj.EventArgs.PartyWith);
            if (res)
                Interlocked.Decrement(ref _totalCountAccumulated);
        }

        public override bool HasPendingNotification(int ownerId, int partyWith)
        {
            var group = NotificationStore.GetOrCreateOnlineGroup(ownerId);
            return group.Item1.HasPending(partyWith);
        }

        public override void ClientDefinitiveDisconnection(EventPattern<TargetEventArgs> obj)
        {
            MoveClientToOffLine(obj.EventArgs.OwnerId);
        }

        //public override void ClientOnlineTimeout(int ownerId)
        //{
        //    MoveClientToOffLine(ownerId);
        //}

        private void MoveClientToOffLine(int ownerId)
        {
            var offLineGroup = NotificationStore.ForceMoveToOffLine(ownerId);

            int partialCount = 0;
            foreach (var item in offLineGroup.GetItemsToPersistence())
            {
                if (item.Value)
                {
                    partialCount++;
                }
                else
                {
                    partialCount--;
                }
            }

            var current = Interlocked.Exchange(ref _totalCountAccumulated, _totalCountAccumulated + partialCount);
            OnCountStore(new CountEventArgs(current));
        }

        public override void ClientConnected(EventPattern<TargetEventArgs> obj)
        {
            var groupResult = NotificationStore.GetOrCreateOnlineGroup(obj.EventArgs.OwnerId);

            if (groupResult.Item2 != NotificationStore.NotificationGroupState.New)
            {
                if (groupResult.Item1.Populated)
                    return;
            }

            groupResult.Item1.ExecuteSync(current => !current.Populated,
            current =>
            {
                var res = ConversasAtivas.PegarConversasAtivas(current.KeyGroupId);
                if (res == null || res.Length <= 0)
                {
                    current.Populated = true;
                    return;
                }

                foreach (var item in res)
                {
                    current.AddPending(item.Curriculo.IdCurriculo, true);
                }

                current.Populated = true;
            });
        }

        public override void ApplicationShutdown(EventPattern<EventArgs> obj)
        {
            BackgroundTaskManager.Run(() =>
                {
                    var toAdd = new List<ConversasAtivas>();
                    foreach (var item in NotificationStore.GetAllOnline())
                    {
                        int ownerId = item.KeyGroupId;
                        var pending = item.GetItemsToPersistence();

                        toAdd.AddRange(pending.Select(a => ConvertToDb(ownerId, a.Key, a.Value)));
                    }

                    foreach (var item in NotificationStore.GetAllOffline())
                    {
                        int ownerId = item.KeyGroupId;
                        var pending = item.GetItemsToPersistence();

                        toAdd.AddRange(pending.Select(a => ConvertToDb(ownerId, a.Key, a.Value)));
                    }

                    try
                    {
                        ConversasAtivas.AtualizarOuInserirVarios(toAdd.ToArray());
                    }
                    catch (Exception ex)
                    {
                        // todo log
                    }
                });
        }
        #endregion

        private ConversasAtivas ConvertToDb(int ownerId, int partyWith, bool pending)
        {
            return new ConversasAtivas
            {
                UsuarioFilialPerfil = new UsuarioFilialPerfil(ownerId),
                Curriculo = new Curriculo(partyWith),
                FlagMensagemPendente = pending
            };
        }

        public void Dispose()
        {
            _disposableSubscription.Dispose();
        }
    }


}