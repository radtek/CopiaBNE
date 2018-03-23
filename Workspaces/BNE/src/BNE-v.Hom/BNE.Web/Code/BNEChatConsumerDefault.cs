using System;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using System.Web;
using BNE.Chat.Core;
using BNE.Chat.Core.EventModel;
using BNE.Chat.Core.Interface;
using BNE.Chat.Core.Notification;
using BNE.Chat.Helper;

namespace BNE.Web.Code
{
    public partial class BNEChatConsumer
    {
        #region [ Standard Fields/Attributes ]
        private readonly ChatStore _chatStore;
        private readonly NotificationHandler _notificationHandler;
        #endregion

        #region [ Public Static Fields/Attributes ]
        public static readonly SetValueOrDefaultFact<HardConfig<int>, int> MessageLengthLimit =
            new HardConfig<int>("chat_message_length_limit", 140).Wrap(a => a.Value);

        public static readonly SetValueOrDefaultFact<HardConfig<int>, int> CacheRequestOnlineContactsTime =
         new SetValueOrDefaultFact<HardConfig<int>, int>(new HardConfig<int>("chat_cache_contatos_online_em_minutos", 10),
                                                         a => a.Value);

        public static readonly SetValueOrDefaultFact<HardConfig<int>, int> QuantityOfMessagesToGetInService =
            new HardConfig<int>("chat_quantity_message_history_from_service", 50).Wrap(a => a.Value);
        #endregion

        #region [ Constructor ]
        public BNEChatConsumer(IChatListener chatManager, ChatStore chatStore, NotificationHandler notificationHandler,
            IClientSimpleSecurity consumerSecurity)
            : base(chatManager, consumerSecurity)
        {
            if (chatStore == null)
                throw new NullReferenceException("chatStore");

            if (notificationHandler == null)
                throw new NullReferenceException("notificationHandler");

            _chatStore = chatStore;
            _notificationHandler = notificationHandler;
        }

        #endregion

        #region [ Properties ]
        public ChatStore ChatStore
        {
            get { return _chatStore; }
        }
        #endregion



        #region [ Basic Methods ]
        protected override void ConnectionOpened(EventPattern<ChatEmptyEventArgs> obj)
        {
            var ownerId = Convert.ToInt32(HttpContext.Current.Request.QueryString["ufp"]);//GetUniqueIdentifierUnsafe();

            if (ownerId == 0)
                return;

            var sessionId = HttpContext.Current.Request.QueryString["connectionToken"]; //HttpContext.Current.Session.SessionID;
            obj.EventArgs.TaskResult = Task.Factory.StartNew(() =>
            {
                var currentSession = ChatStore.GetSession(ownerId);
                bool existentConection = currentSession != null &&
                                         currentSession != sessionId;
                if (existentConection)
                {
                    TryShutDownSession(currentSession);
                }
                var connectionId = obj.EventArgs.Hub.Context.ConnectionId;
                var res = ChatStore.AddConnection(ownerId, sessionId, connectionId);

                if (!existentConection && res == StoreChangedResultType.FirstAddition)
                {
                    _notificationHandler.NotificationMediator.RaiseEnter(new TargetEventArgs { OwnerId = ownerId });
                }
            });
        }

        private bool TryShutDownSession(string currentSession)
        {
            using (var connections = ChatStore.GetConnections(currentSession).Memoize())
            {
                if (!connections.Any())
                    return false;

                var proxy = Manager.GetHubContext().Clients.CreatePrivateSelectionProxy(connections);
                foreach (var item in proxy)
                {
                    item.SendCloseConnection();
                }

                return true;
            }
        }

        protected override void ConnectionClosedUnsafe(EventPattern<ChatEmptyEventArgs> obj)
        {
            var http = HttpContext.Current;

            obj.EventArgs.TaskResult = Task.Factory.StartNew(() =>
            {
                StoreChangedResultType res;

                int ownerId;
                if (http == null || http.Session == null)
                {
                    ownerId = ChatStore.GetOwnerOrDefaultByConnection(obj.EventArgs.Hub.Context.ConnectionId);
                    res = ChatStore.RemoveConnection(obj.EventArgs.Hub.Context.ConnectionId);
                }
                else
                {
                    ownerId = GetUniqueIdentifierUnsafe();
                    if (ownerId <= 0)
                    {
                        ownerId = ChatStore.GetOwnerOrDefaultBySession(http.Session.SessionID);
                        if (ownerId <= 0)
                        {
                            ownerId = ChatStore.GetOwnerOrDefaultBySession(obj.EventArgs.Hub.Context.ConnectionId);
                        }
                    }

                    res = ChatStore.RemoveConnection(http.Session.SessionID, obj.EventArgs.Hub.Context.ConnectionId);
                }

                if (ownerId > 0 && res == StoreChangedResultType.RemovedAll)
                {
                    _notificationHandler.NotificationMediator.RaiseExit(new TargetEventArgs { OwnerId = ownerId });
                }

            });
        }

        protected override void EndOfSessionUnsafe(EventPattern<EventArgs> obj)
        {
            var context = HttpContext.Current;
            HttpSessionStateBase session;

            if (context == null)
            {
                var other = obj.EventArgs as CurrentSessionEventArgs;
                if (other != null && other.Current != null)
                {
                    session = other.Current;
                }
                else
                {
                    return;
                }
            }
            else
            {
                if (context.Session == null)
                    return;

                session = new HttpSessionStateWrapper(context.Session);
            }

            int targetId = GetUniqueIdentifier(session);
            if (targetId <= 0)
                return;

            TryShutDownSession(session.SessionID);
            var res = ChatStore.RemoveSession(session.SessionID);

            if (res == StoreChangedResultType.RemovedAll)
                _notificationHandler.NotificationMediator.RaiseExit(new TargetEventArgs { OwnerId = targetId });

        }
        #endregion
    }

}
