using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reactive.Linq;

namespace AllInTriggers.Helper
{
    public sealed class ReactRoutedEvent<TEventArgs, TTarget> : IDisposable, IReactRoutedEvent<TEventArgs, TTarget>
    {
        #region [ Constructor ]
        public ReactRoutedEvent(Func<TEventArgs, TTarget> directedTargetFactory)
            : this(directedTargetFactory, string.Empty)
        {

        }
        public ReactRoutedEvent(Func<TEventArgs, TTarget> directedTargetFactory, IEqualityComparer<TTarget> comparer)
            : this(directedTargetFactory, comparer, string.Empty)
        {

        }

        public ReactRoutedEvent(Func<TEventArgs, TTarget> directedTargetFactory, string objTitle)
        {
            if (directedTargetFactory == null)
                throw new NullReferenceException("targetFactory");

            _directedTargetFactory = directedTargetFactory;
            _handler = new ConcurrentDictionary<TTarget, IReactEvent<TEventArgs>>();
            _eventTitle = objTitle;
            _broadCast = new ReactEvent<ShootResultArgs<TEventArgs>>("Broadcast of " + objTitle);
        }

        public ReactRoutedEvent(Func<TEventArgs, TTarget> directedTargetFactory, IEqualityComparer<TTarget> comparer, string objTitle)
        {
            if (directedTargetFactory == null)
                throw new NullReferenceException("targetFactory");

            if (comparer == null)
                throw new NullReferenceException("comparer");

            _directedTargetFactory = directedTargetFactory;
            _eventTitle = objTitle;
            _handler = new ConcurrentDictionary<TTarget, IReactEvent<TEventArgs>>(comparer);
            _broadCast = new ReactEvent<ShootResultArgs<TEventArgs>>("Broadcast of " + objTitle);
        }

        #endregion

        #region [ Fields / Attributes ]
        private readonly ConcurrentDictionary<TTarget, IReactEvent<TEventArgs>> _handler;
        private readonly string _eventTitle;

        private Func<TEventArgs, TTarget> _directedTargetFactory;
        private bool _isDisposed;

        private readonly IReactEvent<ShootResultArgs<TEventArgs>> _broadCast;
        #endregion

        #region [ Properties ]
        public string Title
        {
            get { return _eventTitle; }
        }

        public bool IsDisposed
        {
            get { return _isDisposed; }
        }

        public IEnumerable<KeyValuePair<TTarget, IReactEvent<TEventArgs>>> Handlers
        {
            get
            {
                return GetCollectionItems();
            }
        }
        #endregion

        public IObservable<TEventArgs> Observe()
        {
            throw new InvalidOperationException("Not supported");
        }

        public IObservable<ShootResultArgs<TEventArgs>> Broadcast()
        {
            CheckDisposed();
            return _broadCast.Observe();
        }

        public IObservable<TEventArgs> Observe(TTarget observeValue)
        {
            CheckDisposed();

            ReactEvent<TEventArgs> subscriberCreator = null;
            var uniqueHandler = _handler.AddOrUpdate(observeValue, keyF =>
            {
                subscriberCreator = new ReactEvent<TEventArgs>(keyF != null ? keyF.ToString() : "%NULL%");
                return subscriberCreator;
            }, (keyF, old) =>
            {
                return old;
            });

            if (subscriberCreator != null && uniqueHandler != subscriberCreator)
            {
                subscriberCreator.Dispose();
            }

            return uniqueHandler.Observe()
                .Finally(() =>
                {
                    if (((ReactEvent<TEventArgs>)uniqueHandler).State.HasListeners)
                        return;

                    IReactEvent<TEventArgs> removedValue;
                    if (!_handler.TryRemove(observeValue, out removedValue))
                        return;

                    if (subscriberCreator != null && !subscriberCreator.State.HasListeners)
                        return;

                    if (_isDisposed)
                        return;

                    _handler.AddOrUpdate(observeValue, removedValue, (key, old)
                        =>
                        {
                            throw new InvalidOperationException("Not supported multi-thread target's");
                        });
                });
        }

        public void Fire(TEventArgs args)
        {
            CheckDisposed();

            var router = _directedTargetFactory(args);

            IReactEvent<TEventArgs> subscribers;
            if (!_handler.TryGetValue(router, out subscribers))
            {
                var broadArgs = new ShootResultArgs<TEventArgs>(args, false);
                _broadCast.Fire(broadArgs);
                return;
            }

            try
            {
                subscribers.Fire(args);
            }
            catch (Exception ex)
            {
                try
                {
                    var finalizeArgs = new ShootResultArgs<TEventArgs>(args, ex);
                    _broadCast.Fire(finalizeArgs);
                    throw;
                }
                catch (Exception brEx)
                {
                    throw new AggregateException(ex, brEx);
                }
            }

            _broadCast.Fire(new ShootResultArgs<TEventArgs>(args, true));
        }

        public void Dispose()
        {
            _isDisposed = true;
            try
            {
                do
                {
                    var copyItems = _handler.ToArray();

                    foreach (var item in copyItems)
                    {
                        IReactEvent<TEventArgs> current;
                        _handler.TryRemove(item.Key, out current);

                        var disp = current as IDisposable;
                        if (disp != null)
                            disp.Dispose();
                    }

                } while (_handler.Count > 0);
            }
            finally
            {
                var bDisp = (_broadCast as IDisposable);
                if (bDisp != null)
                    bDisp.Dispose();
            }
        }

        private IEnumerable<KeyValuePair<TTarget, IReactEvent<TEventArgs>>> GetCollectionItems()
        {
            foreach (var item in _handler)
            {
                yield return item;
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
