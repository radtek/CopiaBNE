using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Text;
using System.Threading;

namespace AllInTriggers.Helper
{
    public class SchedulerSynchronizationContext : SynchronizationContext
    {
        private readonly IScheduler m_scheduler;

        public SchedulerSynchronizationContext(IScheduler scheduler)
        {
            m_scheduler = scheduler;
        }

        public override void Send(SendOrPostCallback callback, object state)
        {
            throw new NotImplementedException("Too lazy to implemenet synchronous invocation now...");
        }

        public override void Post(SendOrPostCallback callback, object state)
        {
            m_scheduler.Schedule(() => callback.Invoke(state));
        }
    }

   
}
