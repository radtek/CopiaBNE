using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using AllInTriggers;
using AllInTriggers.Core;
using AllInTriggers.Helper;
using BNE.EL;

namespace BNE.Services.Plugins.PluginsEntrada.AllInInput
{

    public class ReactiveFlowInvoker : IDisposable, IReactiveFlowInvoker
    {
        #region [ Fields ]
        private bool _ownerContext;
        private readonly IScheduler _context;
        private readonly IProcessor _processor;
        private readonly ReactSharedEventMediator<string> _mediator;

        private IDisposable _toDispose;
        #endregion

        #region [ Properties ]
        public IScheduler Context
        {
            get { return _context; }
        }

        public bool OwnerOfContext
        {
            get
            {
                return _ownerContext;
            }
        }

        public ReactSharedEventMediator<string> Mediator
        {
            get { return _mediator; }
        }
        #endregion

        #region [ Constructor ]
        public ReactiveFlowInvoker(IProcessor processor)
        {
            if (processor == null)
                throw new NullReferenceException("processor");

            _processor = processor;
            _context = new SyncContextEventLoopScheduler();
            _context.Catch<Exception>(HandleException);
            _ownerContext = true;
            _mediator = new ReactSharedEventMediator<string>();
            _toDispose = CreateSubscriptions();
        }

        private bool HandleException(Exception arg)
        {
            GerenciadorException.GravarExcecao(arg, "#SchedulerError #AllIn");
            return true;
        }

        public ReactiveFlowInvoker(IScheduler scheduler, IProcessor processor)
        {
            if (scheduler == null)
                throw new NullReferenceException("scheduler");

            if (processor == null)
                throw new NullReferenceException("processor");

            _processor = processor;
            _context = scheduler;
            _ownerContext = false;
            _mediator = new ReactSharedEventMediator<string>();
            _toDispose = CreateSubscriptions();
        }

        private IDisposable CreateSubscriptions()
        {
            return _processor.CreateSubscriptions(this);
        }
        #endregion

        public IObservable<DynObj> PostObservable(DynObj data)
        {
            return _mediator.GetOrCreateConsumer<DynObj, DynObj>(EventTitleToAllin.GeneralOutput, a => a, data)
                    .SubscribeOn(_context)
                    .ObserveOn(_context);
        }

        public IDisposable PostRaise(DynObj pair)
        {
            return Context.Schedule(() =>
            {
                RaiseEvent(pair);
            });
        }

        public void SendRaise(DynObj pair)
        {
            if (_ownerContext && Thread.CurrentThread.ManagedThreadId == ((SyncContextEventLoopScheduler)Context).ManagedThreadId)
            {
                RaiseEvent(pair);
            }
            else
            {
                var manualResetEventSlin = new ManualResetEventSlim(false);
                Context.Schedule(() =>
                {
                    try
                    {
                        RaiseEvent(pair);
                    }
                    finally
                    {
                        manualResetEventSlin.Set();
                        manualResetEventSlin.Dispose();
                    }
                });
                manualResetEventSlin.Wait();
            }
        }

        private void RaiseEvent(DynObj pair)
        {
            _mediator.PublishIfExists<DynObj>(EventTitleToAllin.GeneralInput, pair);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_ownerContext)
                return;

            try
            {
                if (Thread.CurrentThread.ManagedThreadId == ((SyncContextEventLoopScheduler)Context).ManagedThreadId)
                {
                    var disp = _context as IDisposable;
                    if (disp != null)
                        disp.Dispose();

                    disp = _toDispose;
                    if (disp != null)
                        _toDispose.Dispose();
                }
            }
            catch
            {
                var manualResetEventSlin = new ManualResetEventSlim(false);
                Context.Schedule(() =>
                {
                    try
                    {
                        Dispose(disposing);
                    }
                    finally
                    {
                        manualResetEventSlin.Set();
                    }
                });
                manualResetEventSlin.Wait();
            }

        }
    }
}
