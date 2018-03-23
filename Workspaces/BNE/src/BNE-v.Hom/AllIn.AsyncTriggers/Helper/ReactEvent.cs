using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;

namespace AllInTriggers.Helper
{
    public struct ReactEventState
    {
        public bool HasListeners { get; set; }
        public bool Raised { get; set; }
        public bool Disposed { get; set; }
    }

    public static class ReactEvent
    {
        public static ReactEvent<TEventArgs> Create<TEventArgs>()
        {
            return new ReactEvent<TEventArgs>();
        }

        public static ReactEvent<TEventArgs> Create<TEventArgs>(string title)
        {
            return new ReactEvent<TEventArgs>(title);
        }

        public static Tuple<ReactEvent<TEventArgs>, IObservable<TEventArgs>> CreateAndObserve<TEventArgs>()
        {
            var react = new ReactEvent<TEventArgs>();
            return new Tuple<ReactEvent<TEventArgs>, IObservable<TEventArgs>>(react, react.Observe());
        }

        public static Tuple<ReactEvent<TEventArgs>, IObservable<TEventArgs>> CreateAndObserve<TEventArgs>(string title)
        {
            var react = new ReactEvent<TEventArgs>(title);
            return new Tuple<ReactEvent<TEventArgs>, IObservable<TEventArgs>>(react, react.Observe());
        }

        public static IObservable<TEventArgs> CreateAndObserve<TEventArgs>(out ReactEvent<TEventArgs> react)
        {
            react = new ReactEvent<TEventArgs>();
            return react.Observe();
        }

        public static IObservable<TEventArgs> CreateAndObserve<TEventArgs>(string title, out ReactEvent<TEventArgs> react)
        {
            react = new ReactEvent<TEventArgs>(title);
            return react.Observe();
        }

        public static ReactEvent<TEventArgs> FireFluent<TEventArgs>(this ReactEvent<TEventArgs> item, object realSender, TEventArgs args)
        {
            item.Fire(args);
            return item;
        }
    }




    public sealed class ReactEvent<TEventArgs> : IDisposable, IReactEvent<TEventArgs>
    {
        #region [ Fields / Attributes ]
        private Action<TEventArgs> _handler;

        private readonly string _eventTitle;
        private Action _disposed;
        private bool _isDisposed;

        private ReactEventState _state;

        #endregion

        #region [ Constructor ]
        public ReactEvent()
        {
        }

        public ReactEvent(string eventTitle)
        {
            this._eventTitle = eventTitle;
        }
        #endregion

        #region [ Properties ]
        public string Title
        {
            get { return _eventTitle; }
        }

        public ReactEventState State
        {
            get { return _state; }
        }
        #endregion

        public IObservable<TEventArgs> Observe()
        {
            CheckDisposed();

            return Observable.Defer(() =>
            {
                CheckDisposed();
                return Observable.FromEvent<TEventArgs>(add => AddInternal(add), rem => Unsubscribe(rem));
            })
            .TakeUntil(Observable.FromEvent(add => Interlocked.Exchange(ref _disposed, _disposed += add),
                                            rem => Interlocked.Exchange(ref _disposed, _disposed -= rem)));
        }

        public IReactEvent<TEventArgs> Subscribe(Action<TEventArgs> invoker)
        {
            if (invoker == null)
                throw new NullReferenceException("invoker");

            CheckDisposed();

            AddInternal(invoker);

            return this;
        }

        private void AddInternal(Action<TEventArgs> invoker)
        {
            var old = _handler;
            var res = Interlocked.CompareExchange(ref _handler, _handler + invoker, null);

            if (res == null)
            {
                _state = new ReactEventState { Raised = _state.Raised, HasListeners = true, Disposed = _isDisposed };
                return;
            }

            if (res.GetInvocationList().Any(obj => (Action<TEventArgs>)obj == invoker))
                return;

            if (!_state.HasListeners)
            {
                _state = new ReactEventState { Raised = _state.Raised, HasListeners = true, Disposed = _isDisposed };
            }

            Interlocked.Exchange(ref _handler, _handler + invoker);

            if (_isDisposed) // re-check to avoid memory leaks
                _handler = null;
        }

        public IReactEvent<TEventArgs> Unsubscribe(Action<TEventArgs> invoker)
        {
            if (invoker == null)
                return this;

            var res = Interlocked.Exchange(ref _handler, _handler - invoker);
            if (res == null || _handler == null)
            {           
                _state = new ReactEventState { HasListeners = false, Raised = _state.Raised, Disposed = _isDisposed };
            }

            return this;
        }

        public void Fire(TEventArgs args)
        {
            CheckDisposed();

            var current = _handler;

            if (current == null)
            {
                if (!_state.Raised)
                {
                    _state = new ReactEventState { Raised = true, HasListeners = _state.HasListeners, Disposed = _isDisposed };
                }
                return;
            }

            try
            {
                foreach (Action<TEventArgs> item in current.GetInvocationList())
                {
                    item(args);
                }
            }
            finally
            {
                if (!_state.Raised)
                {
                    _state = new ReactEventState { Raised = true, HasListeners = _state.HasListeners, Disposed = _isDisposed };
                }
            }
        }

        public void Dispose()
        {
            _isDisposed = true;
            _handler = null;

            var disp = _disposed;
            if (disp == null)
            {
                _state = new ReactEventState { Disposed = true, Raised = _state.Raised, HasListeners = false };
            }
            else
            {
                try
                {
                    disp();
                }
                finally
                {
                    var res = Interlocked.Exchange(ref _disposed, _disposed -= disp);
                    if (res == null)
                    {
                        _state = new ReactEventState { Disposed = true, Raised = _state.Raised, HasListeners = false };
                    }
                    else
                    {
                        try
                        {
                            res();
                        }
                        finally
                        {
                            _handler = null;
                            _disposed = null;

                            _state = new ReactEventState { Disposed = true, Raised = _state.Raised, HasListeners = false };
                        }
                    }
                }
            }
        }
        private void CheckDisposed()
        {
            if (_isDisposed)
            {
                ThrowDisposed();
            }
        }

        private void ThrowDisposed()
        {
            if (string.IsNullOrWhiteSpace(_eventTitle))
                throw new ObjectDisposedException(this.GetType().Name);
            else
                throw new ObjectDisposedException(_eventTitle + " of " + this.GetType().Name);
        }

        void IEventObserver<TEventArgs>.Fire(TEventArgs args)
        {
            this.Fire(args);
        }


    }
}
