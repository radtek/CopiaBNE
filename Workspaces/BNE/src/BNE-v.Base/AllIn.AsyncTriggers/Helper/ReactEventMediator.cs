using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using EqualityComparers;

namespace AllInTriggers.Helper
{

    public class ReactSharedEventMediator<TKey> : ReactRoutedEventMediator<TKey>
    {
        public ReactSharedEventMediator()
        {

        }

        public ReactSharedEventMediator(IEqualityComparer<TKey> comparerKey)
            : base(comparerKey)
        {

        }


        public override IReactEvent<T> CreateIfNotExists<T>(TKey eventKey)
        {
            if (eventKey == null)
                throw new ArgumentNullException("eventKey");

            var current = GetOrAddInternal<T>(NodeFactory<T>(eventKey));
            return current;
        }

        public override IObservable<T> GetConsumer<T>(TKey eventKey)
        {
            if (eventKey == null)
                throw new ArgumentNullException("eventKey");

            var current = GetOrAddInternal<T>(NodeFactory<T>(eventKey));
            if (current.GetType().GetInterfaces().Any(a => a.IsGenericType && a.GetGenericTypeDefinition() == typeof(IReactRoutedEvent<,>)))
            {
                var routed = current as IReactBroadcastEvent<T>;
                if (routed == null)
                    throw new InvalidCastException("IReactBroadcastEvent has a different signature");

                return routed.Broadcast().Select(a => a.OriginalEvent);
            }

            return current.Observe();
        }
    }
    public class ReactRoutedEventMediator<TKey> : ReactEventMediator<TKey>
    {
        protected interface IComposeMediatorNode
        {
            Type AccessorValueType { get; }

            bool HasValidComparer { get; }
        }
        protected class ComposeMediatorNove<TEvent, TValue> : SimpleMediadorNode, IComposeMediatorNode
        {
            public ComposeMediatorNove()
            {
                HandlerType = typeof(TEvent);
            }
            public Func<TEvent, TValue> AccessorValueComparer { get; set; }

            public Type AccessorValueType
            {
                get
                {
                    return typeof(TValue);
                }
            }

            public IEqualityComparer<TValue> ComparerChecker { get; set; }
            public bool HasValidComparer
            {
                get { return ComparerChecker != null; }
            }
        }

        private ThreadLocal<Tuple<Type, Delegate, object>> routeType;

        public ReactRoutedEventMediator()
        {

        }

        public ReactRoutedEventMediator(IEqualityComparer<TKey> comparerKey)
            : base(comparerKey)
        {

        }

        public IReactRoutedEvent<T, TRoutedValue> CreateIfNotExists<T, TRoutedValue>(TKey eventKey, Func<T, TRoutedValue> targetAccessorValue, IEqualityComparer<TRoutedValue> comparer = null)
        {
            if (eventKey == null)
                throw new ArgumentNullException("eventKey");
            if (targetAccessorValue == null)
                throw new ArgumentNullException("targetAccessorValue");

            IReactEvent<T> current;
            if (!TryGetInternal(NodeFactory<T>(eventKey), out current))
            {
                routeType = new ThreadLocal<Tuple<Type, Delegate, object>>(() => new Tuple<Type, Delegate, object>(typeof(TRoutedValue), targetAccessorValue, comparer));
                current = GetOrAddInternal<T>(NodeFactory<T>(eventKey));
                routeType.Dispose();
                routeType = null;
            }

            var routed = current as IReactRoutedEvent<T, TRoutedValue>;
            if (routed == null)
                throw new InvalidCastException("RoutedEvent has a different signature");

            return routed;
        }

        public IObservable<T> GetConsumer<T, TRoutedValue>(TKey eventKey, TRoutedValue targetValue)
        {
            IReactEvent<T> current;
            if (!TryGetInternal(NodeFactory<T>(eventKey), out current))
            {
                throw new KeyNotFoundException("eventKey");
            }

            var routed = current as IReactRoutedEvent<T, TRoutedValue>;
            if (routed == null)
                throw new InvalidCastException("RoutedEvent has a different signature");

            return routed.Observe(targetValue);
        }

        public IObservable<T> GetOrCreateConsumer<T, TRoutedValue>(TKey eventKey, Func<T, TRoutedValue> targetAccessorValue, TRoutedValue targetValue, IEqualityComparer<TRoutedValue> comparer = null)
        {
            var routed = CreateIfNotExists(eventKey, targetAccessorValue, comparer);
            return routed.Observe(targetValue);
        }

        protected override SimpleMediadorNode NodeFactory<T>(TKey eventKey)
        {
            if (routeType == null)
                return base.NodeFactory<T>(eventKey);

            var exactlyType = typeof(ComposeMediatorNove<,>).MakeGenericType(typeof(TKey), typeof(T), routeType.Value.Item1);

            var newNode = Activator.CreateInstance(exactlyType);

            exactlyType.GetProperty("AccessorValueComparer").SetValue(newNode, routeType.Value.Item2, null);
            if (routeType.Value.Item3 != null)
                exactlyType.GetProperty("ComparerChecker").SetValue(newNode, routeType.Value.Item3, null);

            var simple = (SimpleMediadorNode)newNode;
            simple.Key = eventKey;
            return simple;
        }

        protected override IReactEvent<T> EventFactory<T>(SimpleMediadorNode mediadorNode)
        {
            var composer = mediadorNode as IComposeMediatorNode;
            if (composer == null)
                return base.EventFactory<T>(mediadorNode);

            var exactlyType = typeof(ReactRoutedEvent<,>).MakeGenericType(typeof(T), composer.AccessorValueType);

            var nodeType = composer.GetType();
            var accessor = nodeType.GetProperty("AccessorValueComparer").GetValue(composer, null);

            object newEvent;

            if (composer.HasValidComparer)
            {
                var comparer = nodeType.GetProperty("ComparerChecker").GetValue(composer, null);
                newEvent = Activator.CreateInstance(exactlyType, accessor, comparer);
            }
            else
                newEvent = Activator.CreateInstance(exactlyType, accessor);

            return (IReactEvent<T>)newEvent;
        }

        public override void PublishIfExists<T>(TKey eventKey, T args)
        {
            if (eventKey == null)
                throw new ArgumentNullException("eventKey");

            IReactEvent<T> current;
            if (TryGetInternal(NodeFactory<T>(eventKey), out current))
            {
                current.Fire(args);
            }
        }

        public override IObservable<T> GetConsumer<T>(TKey eventKey)
        {
            throw new NotSupportedException("Not supported");
        }

        public override IReactEvent<T> CreateIfNotExists<T>(TKey eventKey)
        {
            throw new NotSupportedException("Not supported");
        }
    }

    public class ReactEventMediator<TKey> : IReactMediatorInvoker<TKey>
    {
        protected class SimpleMediadorNode
        {
            public Type HandlerType { get; set; }
            public TKey Key { get; set; }

            public override int GetHashCode()
            {
                return HandlerType.GetHashCode() ^ Key.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                var other = (SimpleMediadorNode)obj;
                return HandlerType == other.HandlerType && EqualityComparer<TKey>.Default.Equals(Key, other.Key);
            }

            public override string ToString()
            {
                return string.Format("Type'{0}' Key='{1}'", HandlerType.ToString(), Key.ToString());
            }
        }


        private ConcurrentDictionary<SimpleMediadorNode, IEventObservable<object>> _items;

        public IEnumerable<IEventObservable<object>> Handlers
        {
            get { return _items.Select(a => a.Value); }
        }

        public ReactEventMediator()
        {
            _items = new ConcurrentDictionary<SimpleMediadorNode, IEventObservable<object>>();
        }

        public ReactEventMediator(IEqualityComparer<TKey> comparer)
        {
            if (comparer == null)
                throw new NullReferenceException("comparer");

            var equal = new AnonymousEqualityComparer<SimpleMediadorNode>
            {
                Equals = (a, b) => comparer.Equals(a.Key, b.Key),
                GetHashCode = (a) => a.HandlerType.GetHashCode() ^ comparer.GetHashCode(a.Key)
            };
            _items = new ConcurrentDictionary<SimpleMediadorNode, IEventObservable<object>>(equal);
        }

        public virtual void PublishIfExists<T>(TKey eventKey, T args)
        {
            if (eventKey == null)
                throw new ArgumentNullException("eventKey");

            IReactEvent<T> current;
            if (TryGetInternal<T>(NodeFactory<T>(eventKey), out current))
            {
                current.Fire(args);
            }
        }

        public virtual IReactEvent<T> CreateIfNotExists<T>(TKey eventKey)
        {
            if (eventKey == null)
                throw new ArgumentNullException("eventKey");

            var current = GetOrAddInternal<T>(NodeFactory<T>(eventKey));
            return current;
        }

        public virtual IObservable<T> GetConsumer<T>(TKey eventKey)
        {
            if (eventKey == null)
                throw new ArgumentNullException("eventKey");

            var current = GetOrAddInternal<T>(NodeFactory<T>(eventKey));
            return current.Observe();
        }

        protected virtual IReactEvent<T> GetOrAddInternal<T>(SimpleMediadorNode mediadorNode)
        {
            IReactEvent<T> old = null;

            var res = _items.GetOrAdd(mediadorNode, (keyFact) =>
            {
                var current = EventFactory<T>(mediadorNode);
                old = current;
                return (IEventObservable<object>)current;
            });

            if (old != null && old != res)
            {
                var disp = (old as IDisposable);
                if (disp != null)
                    disp.Dispose();
            }
            return (IReactEvent<T>)res;
        }

        public virtual bool TryExclude<T>(IEventObservable<T> handler)
        {
            var toFind = (IEventObservable<object>)handler;
            var pair = _items.FirstOrDefault(a => a.Value == toFind);

            if (_items.TryRemove(pair.Key, out toFind))
                return true;

            return false;
        }
        public virtual bool TryExclude<T>(TKey tKey, out IReactEvent<T> handler)
        {
            IEventObservable<object> current;
            if (_items.TryRemove(NodeFactory<T>(tKey), out current))
            {
                handler = (IReactEvent<T>)current;
                return true;
            }

            handler = null;
            return false;
        }

        public virtual void Clear()
        {
            _items.Clear();
        }

        protected virtual bool TryGetInternal<T>(SimpleMediadorNode mediatorNode, out IReactEvent<T> tValue)
        {
            IEventObservable<object> ox;
            if (_items.TryGetValue(mediatorNode, out ox))
            {
                tValue = (IReactEvent<T>)ox;
                return true;
            }
            tValue = default(IReactEvent<T>);
            return false;
        }

        protected virtual SimpleMediadorNode NodeFactory<T>(TKey eventKey)
        {
            return new SimpleMediadorNode { Key = eventKey, HandlerType = typeof(T) };
        }

        protected virtual IReactEvent<T> EventFactory<T>(SimpleMediadorNode mediadorNode)
        {
            return new ReactEvent<T>(mediadorNode.Key.ToString());
        }

    }
}
