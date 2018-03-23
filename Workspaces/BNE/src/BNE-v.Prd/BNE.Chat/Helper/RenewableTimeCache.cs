using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace BNE.Chat.Helper
{
    public sealed class RenewableTimeCache<TKey, TValue> : ITimeCache<TKey, TValue>
    {
        private class RenovableNode
        {
            private Lazy<TValue> _valueFact;

            public RenovableNode(Func<TValue> valueFact)
            {
                LastAccess = DateTime.Now;
                _valueFact = new Lazy<TValue>(valueFact);
            }
            public DateTime LastAccess { get; set; }

            public TValue Value
            {
                get
                {
                    LastAccess = DateTime.Now;
                    return _valueFact.Value;
                }
            }

            public void SetUpdate(Func<TValue, TValue> func)
            {
                _valueFact = new Lazy<TValue>(() => func(_valueFact.Value));
            }
        }

        #region [ Fields / Attributes ]
        private DateTime _lastClean;

        private readonly TimeSpan _valueTimeToClean;
        private readonly ConcurrentDictionary<TKey, RenovableNode> _cache;

        private readonly object _synchronization = new object();
        private readonly bool _useAccessor;
        private readonly Func<TimeSpan> _accessorToTimeToClean;

        private readonly Action _tryCleanUp;
        private readonly Action<TKey, TValue> _disposeBehavior;
        #endregion

        #region [ Constructor ]
        public RenewableTimeCache(Func<TimeSpan> accessorToTimeToClean, bool cleanUpSynchronized = false, Action<TKey, TValue> removeBehavior = null, IEqualityComparer<TKey> comparer = null)
        {
            if (accessorToTimeToClean == null)
                throw new NullReferenceException("timeToCleanAccessor");

            _lastClean = DateTime.Now;
            _cache = comparer == null ? new ConcurrentDictionary<TKey, RenovableNode>() : new ConcurrentDictionary<TKey, RenovableNode>(comparer);
            
            _useAccessor = true;
            _accessorToTimeToClean = accessorToTimeToClean;

            if (cleanUpSynchronized)
            {
                _tryCleanUp = TryCleanUpSynchronized;
            }
            else
            {
                _tryCleanUp = TryCleanUpConcurrent;
            }

            _disposeBehavior = removeBehavior ?? ((a, b) => { });
        }

        public RenewableTimeCache(TimeSpan valueTimeToClean, bool cleanUpSynchronized = false, Action<TKey, TValue> removeBehavior = null, IEqualityComparer<TKey> comparer = null)
        {
            _lastClean = DateTime.Now;
            _cache = comparer == null ? new ConcurrentDictionary<TKey, RenovableNode>() : new ConcurrentDictionary<TKey, RenovableNode>(comparer);

            _valueTimeToClean = valueTimeToClean;

            if (cleanUpSynchronized)
            {
                _tryCleanUp = TryCleanUpSynchronized;
            }
            else
            {
                _tryCleanUp = TryCleanUpConcurrent;
            }

            _disposeBehavior = removeBehavior ?? ((a, b) => { });
        }
        #endregion

        #region [ Properties ]
        public TimeSpan TimeToClean
        {
            get
            {
                if (_useAccessor)
                    return _accessorToTimeToClean();
                return _valueTimeToClean;
            }
        }
        #endregion

        #region [ Public Methods ]
        public bool TryGetValue(TKey keyId, out TValue result)
        {
            _tryCleanUp();

            RenovableNode getResult;
            var res = _cache.TryGetValue(keyId, out getResult);
            // ReSharper disable once PossibleNullReferenceException
            result = getResult.Value;

            return res;
        }

        public TValue GetOrAdd(TKey keyId, Func<TKey, TValue> valueFactory)
        {
            _tryCleanUp();

            return _cache.GetOrAdd(keyId, keyFactory => new RenovableNode(() => valueFactory(keyFactory))).Value;
        }

        public bool TryRemove(TKey keyId, out TValue value)
        {
            RenovableNode res;
            var removed = _cache.TryRemove(keyId, out res);
            if (removed)
            {
                value = res.Value;
                _disposeBehavior(keyId, value);
            }
            else
            {
                value = default(TValue);
            }

            return removed;
        }

        public TValue AddOrUpdate(TKey keyId, Func<TKey, TValue> valueFactory, Func<TKey, TValue, TValue> update)
        {
            _tryCleanUp();

            return _cache.AddOrUpdate(keyId,
                keyFactory => new RenovableNode(() => valueFactory(keyFactory)),
                (keyFact, oldValue) =>
                {
                    oldValue.LastAccess = DateTime.Now;
                    oldValue.SetUpdate(current => update(keyFact, current));
                    return oldValue;
                }).Value;
        }

        public void Clear()
        {
            KeyValuePair<TKey, RenovableNode>[] keyValuePairs;
            lock (_synchronization)
            {
                keyValuePairs = _cache.ToArray();
                _cache.Clear();
            }

            foreach (var item in keyValuePairs)
            {
                _disposeBehavior(item.Key, item.Value.Value);
            }
        }

        public void TryCleanUp()
        {
            _tryCleanUp();
        }
        #endregion

        #region [ Private Methods ]
        private void TryCleanUpConcurrent()
        {
            if (DateTime.Now - _lastClean < TimeToClean)
                return;

            if (_cache.Count <= 0)
                return;

            if (!Monitor.TryEnter(_synchronization))
                return;

            try
            {
                if (DateTime.Now - _lastClean < TimeToClean)
                    return;

                _lastClean = DateTime.Now;

                var toClean = _cache.Where(keyValuePair => _lastClean - keyValuePair.Value.LastAccess >= TimeToClean);
                foreach (var keyValuePair in toClean)
                {
                    RenovableNode pair;
                    _cache.TryRemove(keyValuePair.Key, out pair);

                    //if (pair.Item2.IsValueCreated)
                    {
                        _disposeBehavior(keyValuePair.Key, pair.Value);
                    }
                }
            }
            finally
            {
                Monitor.Exit(_synchronization);
            }
        }

        private void TryCleanUpSynchronized()
        {
            if (DateTime.Now - _lastClean < TimeToClean)
                return;

            if (_cache.Count <= 0)
                return;

            lock (_synchronization)
            {
                if (DateTime.Now - _lastClean < TimeToClean)
                    return;

                var toClean = _cache.Where(keyValuePair => _lastClean - keyValuePair.Value.LastAccess >= TimeToClean);
                foreach (var keyValuePair in toClean)
                {
                    RenovableNode pair;
                    _cache.TryRemove(keyValuePair.Key, out pair);

                    //if (pair.Item2.IsValueCreated)
                    {
                        _disposeBehavior(keyValuePair.Key, pair.Value);
                    }
                }

                _lastClean = DateTime.Now;
            }
        }
        #endregion
    }
}