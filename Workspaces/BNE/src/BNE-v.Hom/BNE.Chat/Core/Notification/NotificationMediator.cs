using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Runtime;
using System.Threading;
using System.Threading.Tasks;
using BNE.Chat.Core.Base;
using BNE.Chat.Helper;

namespace BNE.Chat.Core.Notification
{
    public class NotificationMediator : IDisposable
    {
        #region [ Fields / Attributes ]
        private object _syncronization = new object();
        private EventHandler<TargetEventArgs> _enter;
        private EventHandler<TargetEventArgs> _exit;
        private EventHandler<NotificationEventArgs> _notificationPending;
        private EventHandler<NotificationEventArgs> _notificationRead;
        #endregion

        #region [ Events ]
        public event EventHandler<TargetEventArgs> Enter
        {
            add { AccessorHelper.AddEvent(ref _enter, value); }
            remove { AccessorHelper.RemoveEvent(ref _enter, value); }
        }

        public event EventHandler<TargetEventArgs> Exit
        {
            add { AccessorHelper.AddEvent(ref _exit, value); }
            remove { AccessorHelper.RemoveEvent(ref _exit, value); }
        }
public event EventHandler ApplicationShutdown;
        #endregion

        #region [ Public Methods ]
        public IObservable<EventPattern<NotificationEventArgs>> NotificationPending(
            ObservablePriority eventPosition = ObservablePriority.Last)
        {
            if (eventPosition == ObservablePriority.First)
                return Observable.FromEventPattern<NotificationEventArgs>(
                            add => AccessorHelper.LockHelper(ref _syncronization, () => _notificationPending = add + _notificationPending),
                            rem => AccessorHelper.LockHelper(ref _syncronization, () => _notificationPending -= rem));

            if (eventPosition == ObservablePriority.Last)
                return Observable.FromEventPattern<NotificationEventArgs>(
                    add => AccessorHelper.LockHelper(ref _syncronization, () => _notificationPending += add),
                    rem => AccessorHelper.LockHelper(ref _syncronization, () => _notificationPending -= rem));

            throw new ArgumentOutOfRangeException("eventPosition");
        }

        public IObservable<EventPattern<NotificationEventArgs>> NotificationRead(
           ObservablePriority eventPosition = ObservablePriority.Last)
        {
            if (eventPosition == ObservablePriority.First)
                return Observable.FromEventPattern<NotificationEventArgs>(
                            add => AccessorHelper.LockHelper(ref _syncronization, () => _notificationRead = add + _notificationRead),
                            rem => AccessorHelper.LockHelper(ref _syncronization, () => _notificationRead -= rem));

            if (eventPosition == ObservablePriority.Last)
                return Observable.FromEventPattern<NotificationEventArgs>(
                    add => AccessorHelper.LockHelper(ref _syncronization, () => _notificationRead += add),
                    rem => AccessorHelper.LockHelper(ref _syncronization, () => _notificationRead -= rem));

            throw new ArgumentOutOfRangeException("eventPosition");
        }


        public void RaiseEnter(TargetEventArgs e)
        {
            OnEnter(e);
        }

        public void RaiseExit(TargetEventArgs e)
        {
            OnExit(e);
        }

        public void RaiseNotificationEntry(NotificationEventArgs e)
        {
            OnNotificationEntry(e);
        }

        public void RaiseNotificationRead(NotificationEventArgs e)
        {
            OnNotificationRead(e);
        }

        public void RaiseApplicationShutdown()
        {
            OnApplicationShutdown();
        }

        public void Dispose()
        {
            _enter = null;
            _exit = null;
            _notificationPending = null;
            _notificationRead = null;
            ApplicationShutdown = null;
        }
        #endregion

        #region [ Protected Methods ]
        protected virtual void OnEnter(TargetEventArgs e)
        {
            var handler = _enter;
            if (handler == null)
                return;

            foreach (var item in handler.GetInvocationList())
            {
                var invoke = item as EventHandler<TargetEventArgs>;
                if (invoke == null)
                    continue;

                invoke(this, e);
                if (e.Handled)
                    break;
            }
        }

        protected virtual void OnExit(TargetEventArgs e)
        {
            var handler = _exit;
            if (handler == null)
                return;

            foreach (var item in handler.GetInvocationList())
            {
                var invoke = item as EventHandler<TargetEventArgs>;
                if (invoke == null)
                    continue;

                invoke(this, e);
                if (e.Handled)
                    break;
            }
        }

        protected virtual void OnNotificationEntry(NotificationEventArgs e)
        {
            var handler = _notificationPending;
            if (handler == null)
                return;

            foreach (var item in handler.GetInvocationList())
            {
                ((EventHandler<NotificationEventArgs>)item)(this, e);
                if (e.Handled)
                    break;
            }
        }

        protected virtual void OnNotificationRead(NotificationEventArgs e)
        {
            var handler = _notificationRead;
            if (handler == null)
                return;

            foreach (var item in handler.GetInvocationList())
            {
                ((EventHandler<NotificationEventArgs>)item)(this, e);
                if (e.Handled)
                    break;
            }
        }

        protected virtual void OnApplicationShutdown()
        {
            var handler = ApplicationShutdown;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }
        #endregion
    }

    public class TargetEventArgs : EventArgs
    {
        public int OwnerId { get; set; }
        public bool Handled { get; set; }
    }

    public class NotificationEventArgs : TargetEventArgs
    {
        public int PartyWith { get; set; }
    }
}
