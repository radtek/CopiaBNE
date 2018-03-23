using System;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using BNE.Chat.Core.Base;
using BNE.Chat.Helper;

namespace BNE.Chat.Core.Notification
{
    public class NotificationManager : IDisposable
    {
        private readonly IDisposable _disposable;

        private readonly NotificationMediator _mediator;
        private readonly Func<NotificationControllerBase> _consumerAccessor;

        public static readonly SetValueOrDefaultFact<HardConfig<int>, int> OffLineTimeout =
  new HardConfig<int>("chat_definicao_offline_definitivo_em_minutos", 2).Wrap(a => a.Value);

  //      public static readonly SetValueOrDefaultFact<HardConfig<int>, int> OnlineTimeout =
  //new HardConfig<int>("chat_definicao_online_timeout_notificacao_em_minutos", 10).Wrap(a => a.Value);

        public NotificationManager(NotificationMediator mediator, Func<NotificationControllerBase> consumerAccessor)
        {
            if (mediator == null)
                throw new NullReferenceException("mediator");

            this._mediator = mediator;
            this._consumerAccessor = consumerAccessor;
            this._disposable = SubscribeSubscriptions();
        }

        protected NotificationControllerBase ConsumerController
        {
            get { return _consumerAccessor(); }
        }

        protected IDisposable SubscribeSubscriptions()
        {
            var multi = new System.Reactive.Disposables.CompositeDisposable();

            multi.Add(CreateConnectedSubscription());
            //multi.Add(CreateOnlineTimeoutSubscription());
            multi.Add(CreateReadResponseNotificationSubscription());
            multi.Add(CreateDefinitiveDisconnectionSubscription());
            multi.Add(CreateNotificationReceivedInDisconnectedSubscription());
            multi.Add(CreateShutDownSubscription());
 
            return multi;
        }

        //private IDisposable CreateOnlineTimeoutSubscription()
        //{
        //    var open = Observable.FromEventPattern<TargetEventArgs>(add => _mediator.Enter += add, rem => _mediator.Enter -= rem)
        //       .Where(a => !a.EventArgs.Handled).Select(a => a.EventArgs.OwnerId);

        //    var mesIn = _mediator.NotificationPending(ObservablePriority.Last).Where(a => !a.EventArgs.Handled).Select(a => a.EventArgs.OwnerId);
        //    var mesOut = _mediator.NotificationRead(ObservablePriority.Last).Where(a => !a.EventArgs.Handled).Select(a => a.EventArgs.OwnerId);

        //    return open.Merge(mesIn.Merge(mesOut))
        //        .Synchronize()
        //        .GroupBy(key => key)
        //        .SelectMany(grp => grp.Throttle(TimeSpan.FromMinutes(OnlineTimeout.Value)))
        //        .Subscribe(ConsumerController.ClientOnlineTimeout);
        //}

        private IDisposable CreateConnectedSubscription()
        {
            var open = Observable.FromEventPattern<TargetEventArgs>(add => _mediator.Enter += add, rem => _mediator.Enter -= rem)
               .Where(a => !a.EventArgs.Handled);

            return open.Subscribe(ConsumerController.ClientConnected);
        }

        private IDisposable CreateShutDownSubscription()
        {
            var shutdown = Observable.FromEventPattern(add => _mediator.ApplicationShutdown += add, rem => _mediator.ApplicationShutdown -= rem);
            return shutdown.Subscribe(this.ConsumerController.ApplicationShutdown);
        }

        private IDisposable CreateNotificationReceivedInDisconnectedSubscription()
        {
            var mesIn = _mediator.NotificationPending(ObservablePriority.Last).Where(a => !a.EventArgs.Handled);
            var mesOut = _mediator.NotificationRead(ObservablePriority.Last).Where(a => !a.EventArgs.Handled);

            var disp = new System.Reactive.Disposables.CompositeDisposable();

            var d1 = mesIn.Subscribe(ConsumerController.NotificationPendingNotHandled);
            disp.Add(d1);

            var d2 = mesOut.Subscribe(ConsumerController.NotificationReadNotHandled);
            disp.Add(d2);

            return disp;
        }


        private IDisposable CreateReadResponseNotificationSubscription()
        {
            var open = Observable.FromEventPattern<TargetEventArgs>(add => _mediator.Enter += add, rem => _mediator.Enter -= rem)
                .Where(a => !a.EventArgs.Handled);
            var close = Observable.FromEventPattern<TargetEventArgs>(add => _mediator.Exit += add, rem => _mediator.Exit -= rem)
                .Where(a => !a.EventArgs.Handled);

            var mesIn = _mediator.NotificationPending(ObservablePriority.First).Where(a => !a.EventArgs.Handled);
            var mesOut = _mediator.NotificationRead(ObservablePriority.First).Where(a => !a.EventArgs.Handled);

            var loggedIn = open.GroupByUntil(
                userId => userId.EventArgs.OwnerId,
                login => close.Where(y => y.EventArgs.OwnerId == login.Key));

            var push = mesIn.Select(obj => Tuple.Create(obj, true)).Merge(mesOut.Select(obj => Tuple.Create(obj, false)));

            Func<IGroupedObservable<int, EventPattern<TargetEventArgs>>,
                    IObservable<Tuple<EventPattern<NotificationEventArgs>, bool>>> notification = session =>
                        push.Where(a => a.Item1.EventArgs.OwnerId == session.Key)
                            .Do(a => a.Item1.EventArgs.Handled = true)
                            .TakeUntil(session.LastOrDefaultAsync());

            var res = loggedIn.SelectMany(notification);

            return res.Subscribe(next =>
            {
                if (next.Item2)
                {
                    ConsumerController.NewOnlineNotificationPending(next.Item1);
                }
                else
                {
                    ConsumerController.NewOnlineNotificationRead(next.Item1);
                }
            });
        }

        private IDisposable CreateDefinitiveDisconnectionSubscription()
        {
            var open = Observable.FromEventPattern<TargetEventArgs>(add => _mediator.Enter += add, rem => _mediator.Enter -= rem)
                .Where(a => !a.EventArgs.Handled);
            var close = Observable.FromEventPattern<TargetEventArgs>(add => _mediator.Exit += add, rem => _mediator.Exit -= rem)
                .Where(a => !a.EventArgs.Handled).ObserveOn(Scheduler.Default).Publish().RefCount();

            var loggedIn = open.GroupByUntil(
                userId => userId.EventArgs.OwnerId,
                login => close.Where(y => y.EventArgs.OwnerId == login.Key))
                .Publish().RefCount();

            var complexClose = Observable.Defer(() => loggedIn.Select(a =>
                close.Where(b => b.EventArgs.OwnerId == a.Key)
                    .Take(1).TakeUntil(a.LastOrDefaultAsync())).Merge()
                );

            var rr = complexClose.ObserveOn(Scheduler.Default).Select(a => Observable.Amb
                (
                    loggedIn.FirstOrDefaultAsync(obj => obj.Key == a.EventArgs.OwnerId).Select(obj => new { a, b = false }),
                    Observable.Return(Unit.Default).Sample(TimeSpan.FromMinutes(OffLineTimeout.Value)).Take(1).Select(obj => new { a, b = true })
                ).Take(1));

            var subs = rr.Merge().Where(a => a.b).Select(a => a.a).Subscribe(ConsumerController.ClientDefinitiveDisconnection);
            return subs;
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}