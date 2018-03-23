using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace BNE.Chat.Core.Notification
{
    /// <summary>
    /// A type that tracks background operations and notifies ASP.NET that they are still in progress.
    /// </summary>
    public sealed class BackgroundTaskManager : IRegisteredObject
    {
        /// <summary>
        /// A cancellation token that is set when ASP.NET is shutting down the app domain.
        /// </summary>
        private readonly CancellationTokenSource shutdown;

        /// <summary>
        /// A countdown event that is incremented each time a task is registered and decremented each time it completes. When it reaches zero, we are ready to shut down the app domain. 
        /// </summary>
        private readonly AsyncCountdownEvent count;

        /// <summary>
        /// A task that completes after <see cref="count"/> reaches zero and the object has been unregistered.
        /// </summary>
        private readonly Task done;

        private BackgroundTaskManager()
        {
            // Start the count at 1 and decrement it when ASP.NET notifies us we're shutting down.
            shutdown = new CancellationTokenSource();
            count = new AsyncCountdownEvent(1);
            shutdown.Token.Register(() => count.Signal(), useSynchronizationContext: false);

            // Register the object and unregister it when the count reaches zero.
            HostingEnvironment.RegisterObject(this);
            done = count.WaitAsync().ContinueWith(_ => HostingEnvironment.UnregisterObject(this), TaskContinuationOptions.ExecuteSynchronously);
        }

        void IRegisteredObject.Stop(bool immediate)
        {
            shutdown.Cancel();
            if (immediate)
                done.Wait();
        }

        /// <summary>
        /// Registers a task with the ASP.NET runtime.
        /// </summary>
        /// <param name="task">The task to register.</param>
        private void Register(Task task)
        {
            count.AddCount();
            task.ContinueWith(_ => count.Signal(), TaskContinuationOptions.ExecuteSynchronously);
        }

        /// <summary>
        /// The background task manager for this app domain.
        /// </summary>
        private static readonly BackgroundTaskManager instance = new BackgroundTaskManager();

        /// <summary>
        /// Gets a cancellation token that is set when ASP.NET is shutting down the app domain.
        /// </summary>
        public static CancellationToken Shutdown { get { return instance.shutdown.Token; } }

        /// <summary>
        /// Executes an <c>async</c> background operation, registering it with ASP.NET.
        /// </summary>
        /// <param name="operation">The background operation.</param>
        public static void Run(Func<Task> operation)
        {
            instance.Register(Task.Factory.StartNew(operation).Unwrap());
        }

        /// <summary>
        /// Executes a background operation, registering it with ASP.NET.
        /// </summary>
        /// <param name="operation">The background operation.</param>
        public static void Run(Action operation)
        {
            instance.Register(Task.Factory.StartNew(operation));
        }
    }

    public sealed class AsyncCountdownEvent
    {
        private readonly TaskCompletionSource<object> _tcs;
        private int _count;
        public int Id
        {
            get
            {
                return this._tcs.Task.Id;
            }
        }
        public int CurrentCount
        {
            get
            {
                return Interlocked.CompareExchange(ref this._count, 0, 0);
            }
        }
        public AsyncCountdownEvent(int count)
        {
            this._tcs = new TaskCompletionSource<object>();
            this._count = count;
        }
        public Task WaitAsync()
        {
            return this._tcs.Task;
        }
        public void Wait()
        {
            this.WaitAsync().Wait();
        }
        public void Wait(CancellationToken cancellationToken)
        {
            Task task = this.WaitAsync();
            if (task.IsCompleted)
            {
                return;
            }
            task.Wait(cancellationToken);
        }
        private bool ModifyCount(int signalCount)
        {
            int num;
            while (true)
            {
                int currentCount = this.CurrentCount;
                if (currentCount == 0)
                {
                    break;
                }
                num = currentCount + signalCount;
                if (num < 0)
                {
                    return false;
                }
                if (Interlocked.CompareExchange(ref this._count, num, currentCount) == currentCount)
                {
                    goto Block_2;
                }
            }
            return false;
        Block_2:
            if (num == 0)
            {
                this._tcs.SetResult(null);
            }
            return true;
        }
        public bool TryAddCount(int signalCount)
        {
            return this.ModifyCount(signalCount);
        }
        public bool TryAddCount()
        {
            return this.TryAddCount(1);
        }
        public bool TrySignal(int signalCount)
        {
            return this.ModifyCount(-signalCount);
        }
        public bool TrySignal()
        {
            return this.TrySignal(1);
        }
        public void AddCount(int signalCount)
        {
            if (!this.ModifyCount(signalCount))
            {
                throw new InvalidOperationException("Cannot increment count.");
            }
        }
        public void AddCount()
        {
            this.AddCount(1);
        }
        public void Signal(int signalCount)
        {
            if (!this.ModifyCount(-signalCount))
            {
                throw new InvalidOperationException("Cannot decrement count.");
            }
        }
        public void Signal()
        {
            this.Signal(1);
        }
    }
}