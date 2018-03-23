using System;
using BNE.Chat.Core.Interface;
using BNE.Chat.Core.Notification;
using Microsoft.AspNet.SignalR.Hubs;

namespace BNE.Chat.Core.Base
{
    public class BNEChatProducerBase
    {
        private readonly IChatListener _manager;
        private readonly ChatStore _chatStore;
        private readonly NotificationHandler _notificationHandler;

        public BNEChatProducerBase(IChatListener manager, ChatStore chatStore, NotificationHandler notificationHandler)
        {
            if (manager == null)
                throw new NullReferenceException("manager");

            if (chatStore == null)
                throw new NullReferenceException("chatStore");

            if (notificationHandler == null)
                throw new NullReferenceException("notificationHandler");

            this._manager = manager;
            this._chatStore = chatStore;
            this._notificationHandler = notificationHandler;
        }

        public IChatListener Manager
        {
            get { return _manager; }
        }

        public ChatStore ChatStore
        {
            get { return _chatStore; }
        }

        public NotificationHandler NotificationHandler
        {
            get { return _notificationHandler; }
        }

        protected virtual IHubConnectionContext GetHubContext()
        {
            return Manager.GetHubContext().Clients;
        }

        protected virtual BNEChatClientProxy GetCurrentMessenger()
        {
            return new BNEChatClientProxy(() => GetHubContext());
        }
    }
}