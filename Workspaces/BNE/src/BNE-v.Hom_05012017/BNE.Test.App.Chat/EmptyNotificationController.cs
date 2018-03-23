using System;
using BNE.Chat.Core.Notification;

namespace BNE.Test.App.Chat
{
    public class EmptyNotificationController : NotificationControllerBase
    {
        public EmptyNotificationController(NotificationStore notificationStore)
            : base(notificationStore)
        {

        }

        public override void NotificationPendingNotHandled(System.Reactive.EventPattern<NotificationEventArgs> obj)
        {
        }

        public override void NotificationReadNotHandled(System.Reactive.EventPattern<NotificationEventArgs> obj)
        {
        }

        public override bool HasPendingNotification(int ownerId, int partyWith)
        {
            return false;
        }

        public override void ClientDefinitiveDisconnection(System.Reactive.EventPattern<TargetEventArgs> obj)
        {
        }

        public override void ClientConnected(System.Reactive.EventPattern<TargetEventArgs> obj)
        {
        }

        public override void NewOnlineNotificationPending(System.Reactive.EventPattern<NotificationEventArgs> eventPattern)
        {
        }

        public override void NewOnlineNotificationRead(System.Reactive.EventPattern<NotificationEventArgs> eventPattern)
        {
        }

        public override void ApplicationShutdown(System.Reactive.EventPattern<EventArgs> obj)
        {
        }
    }
}