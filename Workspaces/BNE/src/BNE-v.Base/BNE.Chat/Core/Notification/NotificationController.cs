using System;
using System.Reactive;

namespace BNE.Chat.Core.Notification
{
    public abstract class NotificationControllerBase
    {
        private readonly NotificationStore _notificationStore;

        protected NotificationControllerBase(NotificationStore notificationStore)
        {
            if (notificationStore == null)
                throw new NullReferenceException("notificationStore");

            this._notificationStore = notificationStore;
        }

        public virtual NotificationStore NotificationStore
        {
            get { return _notificationStore; }
        }

        public abstract void NotificationPendingNotHandled(EventPattern<NotificationEventArgs> obj);
        public abstract void NotificationReadNotHandled(EventPattern<NotificationEventArgs> obj);
        public abstract bool HasPendingNotification(int ownerId, int partyWith);
        public abstract void ClientDefinitiveDisconnection(EventPattern<TargetEventArgs> obj);
        //public abstract void ClientOnlineTimeout(int ownerId);
        public abstract void ClientConnected(EventPattern<TargetEventArgs> obj);
        public abstract void NewOnlineNotificationPending(EventPattern<NotificationEventArgs> eventPattern);
        public abstract void NewOnlineNotificationRead(EventPattern<NotificationEventArgs> eventPattern);
        public abstract void ApplicationShutdown(EventPattern<EventArgs> obj);
    }

 
}