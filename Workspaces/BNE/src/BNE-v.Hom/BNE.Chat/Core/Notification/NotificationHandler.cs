using System;

namespace BNE.Chat.Core.Notification
{
    public sealed class NotificationHandler : IDisposable
    {
        private readonly NotificationManager _notificationManager;
        private readonly NotificationMediator _notificationMediator;
        private readonly NotificationStore _notificationStore;

        public NotificationHandler(NotificationStore notificationStore, NotificationControllerBase notificationConsumer)
        {
            if (notificationConsumer == null)
                throw new NullReferenceException("notificationConsumer");

            if (notificationStore == null)
                throw new NullReferenceException("notificationStore");

            _notificationStore = notificationStore;
            _notificationConsumer = notificationConsumer;

            _notificationMediator = new NotificationMediator();
            _notificationManager = new NotificationManager(_notificationMediator, () => NotificationController);
        }

        private readonly NotificationControllerBase _notificationConsumer;

        public NotificationManager NotificationManager
        {
            get { return _notificationManager; }
        }

        public NotificationMediator NotificationMediator
        {
            get { return _notificationMediator; }
        }

        public NotificationStore NotificationStore
        {
            get { return _notificationStore; }
        }

        public NotificationControllerBase NotificationController
        {
            get { return _notificationConsumer; }
        }

        public void Dispose()
        {
            _notificationMediator.Dispose();
            _notificationManager.Dispose();

            var disp = _notificationConsumer as IDisposable;
            if (disp != null)
            {
                disp.Dispose();
            }
        }
    }
}