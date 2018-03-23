using System;
using System.Threading;
using System.Web;
using System.Web.Routing;
using System.Web.SessionState;
using BNE.Chat.Core.Base;
using BNE.Chat.Core.Hubs;
using BNE.Chat.Core.Interface;
using BNE.Chat.Core.Notification;
using BNE.Chat.Helper;
using Microsoft.AspNet.SignalR;
using BNE.Chat.Core.Security;
using BNE.Chat.Core.EventModel;

namespace BNE.Chat.Core
{
    public sealed class ChatService : IDisposable
    {
        private readonly static HardConfig<string> PathSignalr = new HardConfig<string>("signalr_main_path", @"/signalr/hubs");

        private static bool _configured;
        private static bool _inicialized;

        private static object _syncronization = new object();
        private static ChatService _instance;

        private readonly Lazy<ChatStore> _chatStore;
        private BNEChatConsumerBase _chatConsumer;
        private readonly Lazy<CounterStore> _counterStore;
        private CounterConsumer _counterConsumer;

        private readonly Lazy<NotificationStore> _notificationStore;

        private readonly Lazy<IChatMediator> _chatManager;
        private NotificationHandler _notificationComponent;

        private static INotifySessionEnd _sessionEndSubs;
        private static INotifyAppBeginRequest _beginRequestSubs;
        private static INotifyEndApp _endAppSubs;

        private static IClientSimpleSecurity _simpleSecurity = EmptySecurity.Default;
        private static Func<IChatListener, ChatStore, NotificationHandler, IClientSimpleSecurity, BNEChatConsumerBase> _consumerFactory;
        private static Func<IChatListener, ChatStore, NotificationHandler, BNEChatProducerBase> _producerFactory;
        private static Func<NotificationStore, NotificationControllerBase> _notificationControllerConsumerFactory;

        public static ChatService Instance
        {
            get
            {
                return LazyInitializer.EnsureInitialized(ref _instance, ref _inicialized, ref _syncronization,
                                                         () => new ChatService());
            }
        }

        private ChatService()
        {
            lock (_syncronization)
            {
                if (!_configured)
                {
                    throw new InvalidOperationException("ChatService is not configured.");
                }
            }

            _chatStore = new Lazy<ChatStore>(() => new ChatStore());
            _counterStore = new Lazy<CounterStore>(() => new CounterStore());
            _notificationStore = new Lazy<NotificationStore>(() => new NotificationStore());

            _sessionEndSubs.ReceiveSessionEnd += SessionEndSubsReceiveSessionEnd;
            _beginRequestSubs.ReceiveAppBeginRequest += ApplicationInstance_BeginRequest;
            if (_endAppSubs != null)
                _endAppSubs.ReceiveEndOfApp += EndAppSubsOnReceiveEndOfApp;
            _chatManager = new Lazy<IChatMediator>(() =>
                {
                    var mediator = new ChatMediator();

                    _notificationComponent = new NotificationHandler(_notificationStore.Value, _notificationControllerConsumerFactory(_notificationStore.Value));
                    _chatConsumer = _consumerFactory(mediator, ChatStore, NotificationComponent, SimpleSecurity);
                    _counterConsumer = new CounterConsumer(mediator, CounterStore);

                    return mediator;
                });
        }


        public void IsValid()
        {
            if (!_configured)
            {
                throw new InvalidOperationException("ChatService is not configured.");
            }
        }

        void SessionEndSubsReceiveSessionEnd(object sender, CurrentSessionEventArgs e)
        {
            var sec = SimpleSecurity;
            if (sec != null)
            {
                var current = HttpContext.Current;
                if (current != null)
                {
                    if (!sec.Evaluate(current))
                        return;
                }
            }

            ChatManager.RaiseSessionEnd(sender, e);
        }

        void ApplicationInstance_BeginRequest(object sender, EventArgs e)
        {
            var app = ((HttpApplication)sender);
            if (app.Request.Path.Contains(PathSignalr.Value))
            {
                app.Context.SetSessionStateBehavior(SessionStateBehavior.ReadOnly);
            }
        }

        void EndAppSubsOnReceiveEndOfApp(object sender, EventArgs eventArgs)
        {
            NotificationComponent.NotificationMediator.RaiseApplicationShutdown();
        }

        public IChatMediator ChatManager { get { return _chatManager.Value; } }

        public ChatStore ChatStore { get { return _chatStore.Value; } }

        public NotificationHandler NotificationComponent
        {
            get { return _notificationComponent; }
        }

        public BNEChatConsumerBase ChatConsumer
        {
            get
            {
                if (_chatManager.IsValueCreated)
                {
                    return _chatConsumer;
                }

                if (_chatManager.Value != null)
                {
                    return _chatConsumer;
                }
                return null;
            }
        }

        public CounterStore CounterStore { get { return _counterStore.Value; } }

        public CounterConsumer CounterConsumer
        {
            get
            {
                if (_chatManager.IsValueCreated)
                {
                    return _counterConsumer;
                }

                if (_chatManager.Value != null)
                {
                    return _counterConsumer;
                }

                return null;
            }
        }

        public static IClientSimpleSecurity SimpleSecurity
        {
            get { return _simpleSecurity; }
        }

        public BNEChatProducerBase GetChatProducer()
        {
            return _producerFactory(ChatManager, ChatStore, NotificationComponent);
        }

        public void Dispose()
        {
            if (_chatStore.IsValueCreated)
                ChatStore.CleanAll();

            if (_chatManager.IsValueCreated)
                ChatManager.Dispose();

            var cons = ChatConsumer as IDisposable;
            if (cons != null)
                try
                {

                    ChatConsumer.Dispose();
                }
                catch (Exception)
                {
                }

            cons = CounterConsumer;
            if (cons != null)
                try
                {
                    CounterConsumer.Dispose();
                }
                catch (Exception)
                {
                }

            try
            {
                NotificationComponent.Dispose();
            }
            catch (Exception)
            {

            }

            _beginRequestSubs.ReceiveAppBeginRequest -= ApplicationInstance_BeginRequest;
            _sessionEndSubs.ReceiveSessionEnd -= SessionEndSubsReceiveSessionEnd;
            _endAppSubs.ReceiveEndOfApp -= EndAppSubsOnReceiveEndOfApp;
        }

        public static void Configure(
            Func<IChatListener, ChatStore, NotificationHandler, IClientSimpleSecurity, BNEChatConsumerBase> uniqueChatConsumer,
            Func<IChatListener, ChatStore, NotificationHandler, BNEChatProducerBase> producerFactory,
            Func<NotificationStore, NotificationControllerBase> uniqueNoficationConsumer,
            INotifyAppBeginRequest beginRequest,
            INotifySessionEnd sessionEnd, INotifyEndApp appEnd,
            IClientSimpleSecurity simpleSecurity = null)
        {
            if (uniqueChatConsumer == null)
                throw new NullReferenceException("uniqueChatConsumer");

            if (producerFactory == null)
                throw new NullReferenceException("producerFactory");

            if (sessionEnd == null)
                throw new NullReferenceException("notifier");

            if (beginRequest == null)
                throw new NullReferenceException("beginRequest");

            if (_configured)
                throw new InvalidOperationException("ChatService already configured.");

            if (uniqueNoficationConsumer == null)
                throw new NullReferenceException("uniqueNotificaitonConsumer");

            RouteTable.Routes.MapHubs(PathSignalr.Value, new HubConfiguration() { EnableCrossDomain = true });


            GlobalHost.DependencyResolver.Register(typeof(ChatHub), () =>
            {
                if (_configured)
                    return new ChatHub(Instance.ChatManager);

                return new ChatHub(null);
            });
            GlobalHost.DependencyResolver.Register(typeof(CounterHub), () =>
            {
                if (_configured)
                    return new CounterHub(Instance.CounterStore);

                return new CounterHub(null);
            });

            _sessionEndSubs = sessionEnd;
            _beginRequestSubs = beginRequest;
            _endAppSubs = appEnd;
            _simpleSecurity = simpleSecurity ?? EmptySecurity.Default;
            _consumerFactory = uniqueChatConsumer;
            _producerFactory = producerFactory;
            _notificationControllerConsumerFactory = uniqueNoficationConsumer;

            _configured = true;

            if (Instance.ChatManager == null)
            {
                throw new NullReferenceException("Instance ChatManager"); // Inicialize
            }
        }


    }

}