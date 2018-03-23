using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Text;
using System.Threading;

namespace AllInTriggers.Helper
{
    public sealed class SyncContextEventLoopScheduler : IScheduler
    {
        private readonly EventLoopScheduler m_scheduler;
        private int managedThreadId;

        public int ManagedThreadId
        {
            get { return managedThreadId; }
        }

        public SyncContextEventLoopScheduler()
        {
            m_scheduler = new EventLoopScheduler();
            SetSyncContext();
        }

        public SyncContextEventLoopScheduler(Func<ThreadStart, Thread> factory)
        {
            m_scheduler = new EventLoopScheduler(factory);
            SetSyncContext();
        }

        #region IScheduler Members

        public IDisposable Schedule<TState>(TState state, Func<IScheduler, TState, IDisposable> action)
        {
            return m_scheduler.Schedule(state, action);
        }

        public IDisposable Schedule<TState>(TState state, TimeSpan dueTime, Func<IScheduler, TState, IDisposable> action)
        {
            return m_scheduler.Schedule(state, dueTime, action);
        }

        public IDisposable Schedule<TState>(TState state, DateTimeOffset dueTime,
                                            Func<IScheduler, TState, IDisposable> action)
        {
            return m_scheduler.Schedule(state, dueTime, action);
        }

        public DateTimeOffset Now
        {
            get { return m_scheduler.Now; }
        }

        #endregion

        private void SetSyncContext()
        {
            var sync = new ManualResetEventSlim(false);
            m_scheduler.Schedule(() =>
            {
                var syncContext = new SchedulerSynchronizationContext(this);
                SynchronizationContext.SetSynchronizationContext(syncContext);
                this.managedThreadId = Thread.CurrentThread.ManagedThreadId;
                sync.Set();
            });
            sync.Wait();
        }

        public void Dispose()
        {
            m_scheduler.Dispose();
        }
    }
}
