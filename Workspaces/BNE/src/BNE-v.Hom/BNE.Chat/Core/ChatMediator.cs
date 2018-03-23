using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using BNE.Chat.Core.Base;
using BNE.Chat.Core.EventModel;
using BNE.Chat.Core.Hubs;
using BNE.Chat.Core.Interface;
using BNE.Chat.Helper;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace BNE.Chat.Core
{
    public class ChatMediator : IChatMediator
    {
        #region [ Fields / Attributes ]
        private bool _disposed;

        private EventHandler _onDisposed;

        private object _syncronization = new object();
        #endregion

        #region [ Contructor ]

        public ChatMediator()
        {

        }

        #endregion

        #region [ Core Methods ]
        private EventHandler<ChatEmptyEventArgs> _keepAlive;
        private EventHandler<ChatDefaultEventArgs> _typingSignal;
        private EventHandler<ChatDefaultEventArgs> _closeChat;
        private EventHandler<ChatDefaultEventArgs> _sendNotificationRead;
        private EventHandler<ChatDefaultEventArgs> _deleteChat;

        private EventHandler<ChatResultEventArgs> _getMoreInfo;
        private EventHandler<ChatResultEventArgs> _getOnlineContacts;
      
        private EventHandler<ChatResultEventArgs> _requestChatHistory;
        private EventHandler<ChatResultEventArgs> _sendMessage;
        private EventHandler<ChatResultEventArgs> _requestNewChat;

        private EventHandler _endOfSession;


        public void RaiseSessionEnd(object realSender, EventArgs realArgs)
        {
            AccessorHelper.InvokeIfIsNotNull(this._endOfSession, a => a(realSender, realArgs));
        }

        public IObservable<EventPattern<EventArgs>> OnEndOfSession(ObservablePriority eventPosition = ObservablePriority.Last)
        {
            ThrowIfDisposed();

            if (eventPosition == ObservablePriority.First)
                return Observable.FromEventPattern(
                            add => AccessorHelper.LockHelper(ref _syncronization, () => _endOfSession = add + _endOfSession),
                            rem => AccessorHelper.LockHelper(ref _syncronization, () => _endOfSession -= rem));

            if (eventPosition == ObservablePriority.Last)
                return Observable.FromEventPattern(
                    add => AccessorHelper.LockHelper(ref _syncronization, () => _endOfSession += add),
                    rem => AccessorHelper.LockHelper(ref _syncronization, () => _endOfSession -= rem));

            throw new ArgumentOutOfRangeException("eventPosition");
        }


        public Task RaiseClientTyping(Hub chatHub, object receiveArgs)
        {
            var args = new ChatDefaultEventArgs(chatHub, receiveArgs);
            AccessorHelper.InvokeIfIsNotNull(this._typingSignal, a => a(this, args));
            return args.TaskResult;
        }

        public IObservable<EventPattern<ChatDefaultEventArgs>> OnClientTyping(ObservablePriority eventPosition = ObservablePriority.Last)
        {
            ThrowIfDisposed();

            if (eventPosition == ObservablePriority.First)
                return Observable.FromEventPattern<ChatDefaultEventArgs>(
                            add => AccessorHelper.LockHelper(ref _syncronization, () => _typingSignal = add + _typingSignal),
                            rem => AccessorHelper.LockHelper(ref _syncronization, () => _typingSignal -= rem));

            if (eventPosition == ObservablePriority.Last)
                return Observable.FromEventPattern<ChatDefaultEventArgs>(
                    add => AccessorHelper.LockHelper(ref _syncronization, () => _typingSignal += add),
                    rem => AccessorHelper.LockHelper(ref _syncronization, () => _typingSignal -= rem));

            throw new ArgumentOutOfRangeException("eventPosition");
        }

        public Task RaiseKeepAlive(Hub chatHub)
        {
            var args = new ChatEmptyEventArgs(chatHub);
            AccessorHelper.InvokeIfIsNotNull(this._keepAlive, a => a(this, args));
            return args.TaskResult;
        }

        public IObservable<EventPattern<ChatEmptyEventArgs>> OnKeepAlive(ObservablePriority eventPosition = ObservablePriority.Last)
        {
            ThrowIfDisposed();

            if (eventPosition == ObservablePriority.First)
                return Observable.FromEventPattern<ChatEmptyEventArgs>(
                            add => AccessorHelper.LockHelper(ref _syncronization, () => _keepAlive = add + _keepAlive),
                            rem => AccessorHelper.LockHelper(ref _syncronization, () => _keepAlive -= rem));

            if (eventPosition == ObservablePriority.Last)
                return Observable.FromEventPattern<ChatEmptyEventArgs>(
                    add => AccessorHelper.LockHelper(ref _syncronization, () => _keepAlive += add),
                    rem => AccessorHelper.LockHelper(ref _syncronization, () => _keepAlive -= rem));

            throw new ArgumentOutOfRangeException("eventPosition");
        }

        public Task<ISignalRGenericResult> RaiseSendMessageTo(Hub chatHub, object chatValueArgs)
        {
            var args = new ChatResultEventArgs(chatHub,chatValueArgs);
            AccessorHelper.InvokeIfIsNotNull(this._sendMessage, a => a(this, args));
            return args.TaskValueResult;
        }

        public IObservable<EventPattern<ChatResultEventArgs>> OnSendMessageTo(ObservablePriority eventPosition = ObservablePriority.Last)
        {
            ThrowIfDisposed();

            if (eventPosition == ObservablePriority.First)
                return Observable.FromEventPattern<ChatResultEventArgs>(
                            add => AccessorHelper.LockHelper(ref _syncronization, () => _sendMessage = add + _sendMessage),
                            rem => AccessorHelper.LockHelper(ref _syncronization, () => _sendMessage -= rem));

            if (eventPosition == ObservablePriority.Last)
                return Observable.FromEventPattern<ChatResultEventArgs>(
                    add => AccessorHelper.LockHelper(ref _syncronization, () => _sendMessage += add),
                    rem => AccessorHelper.LockHelper(ref _syncronization, () => _sendMessage -= rem));

            throw new ArgumentOutOfRangeException("eventPosition");
        }

        public Task<ISignalRGenericResult> RaiseRequestHistory(Hub chatHub, object chatValueArgs)
        {
            var args = new ChatResultEventArgs(chatHub, chatValueArgs);
            AccessorHelper.InvokeIfIsNotNull(this._requestChatHistory, a => a(this, args));
            return args.TaskValueResult;
        }

        public IObservable<EventPattern<ChatResultEventArgs>> OnRequestHistory(ObservablePriority eventPosition = ObservablePriority.Last)
        {
            ThrowIfDisposed();

            if (eventPosition == ObservablePriority.First)
                return Observable.FromEventPattern<ChatResultEventArgs>(
                            add => AccessorHelper.LockHelper(ref _syncronization, () => _requestChatHistory = add + _requestChatHistory),
                            rem => AccessorHelper.LockHelper(ref _syncronization, () => _requestChatHistory -= rem));

            if (eventPosition == ObservablePriority.Last)
                return Observable.FromEventPattern<ChatResultEventArgs>(
                    add => AccessorHelper.LockHelper(ref _syncronization, () => _requestChatHistory += add),
                    rem => AccessorHelper.LockHelper(ref _syncronization, () => _requestChatHistory -= rem));

            throw new ArgumentOutOfRangeException("eventPosition");
        }

        public Task<ISignalRGenericResult> RaiseRequestNewChatWith(Hub chatHub, object chatValueArgs)
        {
            var args = new ChatResultEventArgs(chatHub);
            AccessorHelper.InvokeIfIsNotNull(this._requestNewChat, a => a(this, args));
            return args.TaskValueResult;
        }

        public IObservable<EventPattern<ChatResultEventArgs>> OnRequestNewChatWith(ObservablePriority eventPosition = ObservablePriority.Last)
        {
            ThrowIfDisposed();

            if (eventPosition == ObservablePriority.First)
                return Observable.FromEventPattern<ChatResultEventArgs>(
                            add => AccessorHelper.LockHelper(ref _syncronization, () => _requestNewChat = add + _requestNewChat),
                            rem => AccessorHelper.LockHelper(ref _syncronization, () => _requestNewChat -= rem));

            if (eventPosition == ObservablePriority.Last)
                return Observable.FromEventPattern<ChatResultEventArgs>(
                    add => AccessorHelper.LockHelper(ref _syncronization, () => _requestNewChat += add),
                    rem => AccessorHelper.LockHelper(ref _syncronization, () => _requestNewChat -= rem));

            throw new ArgumentOutOfRangeException("eventPosition");
        }

        public Task<ISignalRGenericResult> RaiseGetMoreInfo(Hub chatHub, object receiveArgs)
        {
            var args = new ChatResultEventArgs(chatHub, receiveArgs);
            AccessorHelper.InvokeIfIsNotNull(this._getMoreInfo, a => a(this, args));
            return args.TaskValueResult;
        }

        public IObservable<EventPattern<ChatResultEventArgs>> OnGetMoreInfo(ObservablePriority eventPosition = ObservablePriority.Last)
        {
            ThrowIfDisposed();

            if (eventPosition == ObservablePriority.First)
                return Observable.FromEventPattern<ChatResultEventArgs>(
                            add => AccessorHelper.LockHelper(ref _syncronization, () => _getMoreInfo = add + _getMoreInfo),
                            rem => AccessorHelper.LockHelper(ref _syncronization, () => _getMoreInfo -= rem));

            if (eventPosition == ObservablePriority.Last)
                return Observable.FromEventPattern<ChatResultEventArgs>(
                    add => AccessorHelper.LockHelper(ref _syncronization, () => _getMoreInfo += add),
                    rem => AccessorHelper.LockHelper(ref _syncronization, () => _getMoreInfo -= rem));

            throw new ArgumentOutOfRangeException("eventPosition");
        }


        public Task<ISignalRGenericResult> RaiseRequestOnlineContacts(Hub chatHub)
        {
            var args = new ChatResultEventArgs(chatHub);
            AccessorHelper.InvokeIfIsNotNull(this._getOnlineContacts, a => a(this, args));
            return args.TaskValueResult;
        }

        public IObservable<EventPattern<ChatResultEventArgs>> OnRequestOnlineContacts(ObservablePriority eventPosition = ObservablePriority.Last)
        {
            ThrowIfDisposed();

            if (eventPosition == ObservablePriority.First)
                return Observable.FromEventPattern<ChatResultEventArgs>(
                            add => AccessorHelper.LockHelper(ref _syncronization, () => _getOnlineContacts = add + _getOnlineContacts),
                            rem => AccessorHelper.LockHelper(ref _syncronization, () => _getOnlineContacts -= rem));

            if (eventPosition == ObservablePriority.Last)
                return Observable.FromEventPattern<ChatResultEventArgs>(
                    add => AccessorHelper.LockHelper(ref _syncronization, () => _getOnlineContacts += add),
                    rem => AccessorHelper.LockHelper(ref _syncronization, () => _getOnlineContacts -= rem));

            throw new ArgumentOutOfRangeException("eventPosition");
        }


        public Task RaiseCloseChat(Hub chatHub, object chatValueArgs)
        {
            var args = new ChatDefaultEventArgs(chatHub, chatValueArgs);
            AccessorHelper.InvokeIfIsNotNull(this._closeChat, a => a(this, args));
            return args.TaskResult;
        }

        public IObservable<EventPattern<ChatDefaultEventArgs>> OnCloseChat(ObservablePriority eventPosition = ObservablePriority.Last)
        {
            ThrowIfDisposed();

            if (eventPosition == ObservablePriority.First)
                return Observable.FromEventPattern<ChatDefaultEventArgs>(
                            add => AccessorHelper.LockHelper(ref _syncronization, () => _closeChat = add + _closeChat),
                            rem => AccessorHelper.LockHelper(ref _syncronization, () => _closeChat -= rem));

            if (eventPosition == ObservablePriority.Last)
                return Observable.FromEventPattern<ChatDefaultEventArgs>(
                    add => AccessorHelper.LockHelper(ref _syncronization, () => _closeChat += add),
                    rem => AccessorHelper.LockHelper(ref _syncronization, () => _closeChat -= rem));

            throw new ArgumentOutOfRangeException("eventPosition");
        }


        public IObservable<EventPattern<ChatDefaultEventArgs>> OnSendReadNotification(ObservablePriority eventPosition = ObservablePriority.Last)
        {
            ThrowIfDisposed();

            if (eventPosition == ObservablePriority.First)
                return Observable.FromEventPattern<ChatDefaultEventArgs>(
                            add => AccessorHelper.LockHelper(ref _syncronization, () => _sendNotificationRead = add + _sendNotificationRead),
                            rem => AccessorHelper.LockHelper(ref _syncronization, () => _sendNotificationRead -= rem));

            if (eventPosition == ObservablePriority.Last)
                return Observable.FromEventPattern<ChatDefaultEventArgs>(
                    add => AccessorHelper.LockHelper(ref _syncronization, () => _sendNotificationRead += add),
                    rem => AccessorHelper.LockHelper(ref _syncronization, () => _sendNotificationRead -= rem));

            throw new ArgumentOutOfRangeException("eventPosition");
        }


        public Task RaiseSendReadNotification(Hub chatHub, object receiveArgs)
        {
            var args = new ChatDefaultEventArgs(chatHub, receiveArgs);
            AccessorHelper.InvokeIfIsNotNull(_sendNotificationRead, instance => instance(this, args));
            return args.TaskResult;
        }

        public IObservable<EventPattern<ChatDefaultEventArgs>> OnDeleteChat(ObservablePriority eventPosition = ObservablePriority.Last)
        {
            ThrowIfDisposed();

            if (eventPosition == ObservablePriority.First)
                return Observable.FromEventPattern<ChatDefaultEventArgs>(
                            add => AccessorHelper.LockHelper(ref _syncronization, () => _deleteChat = add + _deleteChat),
                            rem => AccessorHelper.LockHelper(ref _syncronization, () => _deleteChat -= rem));

            if (eventPosition == ObservablePriority.Last)
                return Observable.FromEventPattern<ChatDefaultEventArgs>(
                    add => AccessorHelper.LockHelper(ref _syncronization, () => _deleteChat += add),
                    rem => AccessorHelper.LockHelper(ref _syncronization, () => _deleteChat -= rem));

            throw new ArgumentOutOfRangeException("eventPosition");
        }

        public Task RaiseDeleteChat(Hub chatHub, object chatValueArgs)
        {
            var args = new ChatDefaultEventArgs(chatHub, chatValueArgs);
            AccessorHelper.InvokeIfIsNotNull(_deleteChat, instance => instance(this, args));
            return args.TaskResult;
        }

        public IHubContext GetHubContext()
        {
            var chatHub = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();

            if (chatHub == null)
                throw new NullReferenceException("chatHub");

            return chatHub;
        }

        #endregion

        #region [ Default Methods ]
        private EventHandler<ChatEmptyEventArgs> _newConnection;
        private EventHandler<ChatEmptyEventArgs> _closeConnection;
        private EventHandler<ChatEmptyEventArgs> _reconnectConnection;

        public Task RaiseNewConnection(Hub currentHub)
        {
            var args = new ChatEmptyEventArgs(currentHub);
            AccessorHelper.InvokeIfIsNotNull(_newConnection, instance => instance(this, args));
            return args.TaskResult;
        }

        public Task RaiseReconnectConnection(Hub currentHub)
        {
            var args = new ChatEmptyEventArgs(currentHub);
            AccessorHelper.InvokeIfIsNotNull(_reconnectConnection, instance => instance(this, args));
            return args.TaskResult;
        }

        public Task RaiseCloseConnection(Hub currentHub)
        {
            var args = new ChatEmptyEventArgs(currentHub);
            AccessorHelper.InvokeIfIsNotNull(_closeConnection, instance => instance(this, args));
            return args.TaskResult;
        }

        public IObservable<EventPattern<ChatEmptyEventArgs>> OnNewConnection(ObservablePriority eventPosition = ObservablePriority.Last)
        {
            ThrowIfDisposed();

            if (eventPosition == ObservablePriority.First)
                return Observable.FromEventPattern<ChatEmptyEventArgs>(
                            add => AccessorHelper.LockHelper(ref _syncronization, () => _newConnection = add + _newConnection),
                            rem => AccessorHelper.LockHelper(ref _syncronization, () => _newConnection -= rem));

            if (eventPosition == ObservablePriority.Last)
                return Observable.FromEventPattern<ChatEmptyEventArgs>(
                    add => AccessorHelper.LockHelper(ref _syncronization, () => _newConnection += add),
                    rem => AccessorHelper.LockHelper(ref _syncronization, () => _newConnection -= rem));

            throw new ArgumentOutOfRangeException("eventPosition");
        }

        public IObservable<EventPattern<ChatEmptyEventArgs>> OnCloseConnection(ObservablePriority eventPosition = ObservablePriority.Last)
        {
            ThrowIfDisposed();

            if (eventPosition == ObservablePriority.First)
                return Observable.FromEventPattern<ChatEmptyEventArgs>(
                            add => AccessorHelper.LockHelper(ref _syncronization, () => _closeConnection = add + _closeConnection),
                            rem => AccessorHelper.LockHelper(ref _syncronization, () => _closeConnection -= rem));

            if (eventPosition == ObservablePriority.Last)
                return Observable.FromEventPattern<ChatEmptyEventArgs>(
                    add => AccessorHelper.LockHelper(ref _syncronization, () => _closeConnection += add),
                    rem => AccessorHelper.LockHelper(ref _syncronization, () => _closeConnection -= rem));

            throw new ArgumentOutOfRangeException("eventPosition");
        }

        public IObservable<EventPattern<ChatEmptyEventArgs>> OnReconnectConnection(ObservablePriority eventPosition = ObservablePriority.Last)
        {
            ThrowIfDisposed();

            if (eventPosition == ObservablePriority.First)
                return Observable.FromEventPattern<ChatEmptyEventArgs>(
                            add => AccessorHelper.LockHelper(ref _syncronization, () => _reconnectConnection = add + _reconnectConnection),
                            rem => AccessorHelper.LockHelper(ref _syncronization, () => _reconnectConnection -= rem));

            if (eventPosition == ObservablePriority.Last)
                return Observable.FromEventPattern<ChatEmptyEventArgs>(
                    add => AccessorHelper.LockHelper(ref _syncronization, () => _reconnectConnection += add),
                    rem => AccessorHelper.LockHelper(ref _syncronization, () => _reconnectConnection -= rem));

            throw new ArgumentOutOfRangeException("eventPosition");
        }
        #endregion

        #region  [ Dispose Methods ]
        public void Dispose()
        {
            _disposed = true;

            _requestNewChat = null;
            _requestChatHistory = null;
            _sendMessage = null;
            _keepAlive = null;
            _typingSignal = null;

            _newConnection = null;
            _reconnectConnection = null;
            _closeConnection = null;
            _sendNotificationRead = null;

            var disposedEvent = _onDisposed;
            if (disposedEvent != null)
            {
                try
                {
                    disposedEvent(this, EventArgs.Empty);
                }
                catch (Exception)
                {
                }
            }
        }

        public IObservable<IChatListener> OnDisposed()
        {
            if (_disposed)
            {
                return Observable.Return(this);
            }

            return
                Observable.FromEventPattern(add => _onDisposed += add, rem => _onDisposed -= rem)
                    .Select(a => (ChatMediator)a.Sender);
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(this.GetType().Name);
            }
        }
        #endregion

    }
}
