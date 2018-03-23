using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace BNE.Chat.Helper
{
    public sealed class CooperativeTimeCache<TKey, TValue> : ITimeCache<TKey, TValue>
    {
        #region [ Fields / Attributes ]
        private DateTime _lastClean;

        private readonly TimeSpan _valueTimeToClean;
        private readonly ConcurrentDictionary<TKey, Tuple<DateTime, Lazy<TValue>>> _cache;

        private readonly object _synchronization = new object();
        private readonly bool _useAccessor;
        private readonly Func<TimeSpan> _accessorToTimeToClean;

        private readonly Action _tryCleanUp;
        private readonly Action<TKey, TValue> _disposeBehavior;
        #endregion

        #region [ Constructor ]
        public CooperativeTimeCache(Func<TimeSpan> accessorToTimeToClean, bool cleanUpSynchronized = false, Action<TKey, TValue> removeBehavior = null, IEqualityComparer<TKey> comparer = null)
        {
            if (accessorToTimeToClean == null)
                throw new NullReferenceException("timeToCleanAccessor");

            _useAccessor = true;
            _lastClean = DateTime.Now;
            _cache = comparer == null ? new ConcurrentDictionary<TKey, Tuple<DateTime, Lazy<TValue>>>() : new ConcurrentDictionary<TKey, Tuple<DateTime, Lazy<TValue>>>(comparer);

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

        public CooperativeTimeCache(TimeSpan valueTimeToClean, bool cleanUpSynchronized = false, Action<TKey, TValue> removeBehavior = null, IEqualityComparer<TKey> comparer = null)
        {
            _lastClean = DateTime.Now;
            _cache = comparer == null ? new ConcurrentDictionary<TKey, Tuple<DateTime, Lazy<TValue>>>() : new ConcurrentDictionary<TKey, Tuple<DateTime, Lazy<TValue>>>(comparer);

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

            Tuple<DateTime, Lazy<TValue>> getResult;
            var res = _cache.TryGetValue(keyId, out getResult);

            if (res)
                result = getResult.Item2.Value;
            else
                result = default(TValue);

            return res;
        }

        public TValue GetOrAdd(TKey keyId, Func<TKey, TValue> valueFactory)
        {
            _tryCleanUp();

            return _cache.GetOrAdd(keyId, keyFactory =>
            {
                var lazy = new Lazy<TValue>(() => valueFactory(keyFactory));
                return new Tuple<DateTime, Lazy<TValue>>(DateTime.Now, lazy);
            }).Item2.Value;
        }

        public bool TryRemove(TKey keyId, out TValue value)
        {
            Tuple<DateTime, Lazy<TValue>> res;

            var removed = _cache.TryRemove(keyId, out res);
            if (removed)
            {
                if (res.Item2.IsValueCreated)
                {
                    value = res.Item2.Value;
                    _disposeBehavior(keyId, value);
                }
                else
                {
                    value = default(TValue);
                }
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

            return _cache.AddOrUpdate(keyId, keyFact =>
             {
                 var lazy = new Lazy<TValue>(() => valueFactory(keyFact));
                 return new Tuple<DateTime, Lazy<TValue>>(DateTime.Now, lazy);
             }, (keyFact, oldValue) =>
             {
                 var res = new Lazy<TValue>(() => update(keyFact, oldValue.Item2.Value));
                 return new Tuple<DateTime, Lazy<TValue>>(DateTime.Now, res);
             }).Item2.Value;
        }

        public void Clear()
        {
            KeyValuePair<TKey, Tuple<DateTime, Lazy<TValue>>>[] currentPair;
            lock (_synchronization)
            {
                currentPair = _cache.ToArray();
                _cache.Clear();
            }

            foreach (var item in currentPair)
            {
                _disposeBehavior(item.Key, item.Value.Item2.Value);
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

                var toClean = _cache.Where(keyValuePair => _lastClean - keyValuePair.Value.Item1 >= TimeToClean);
                foreach (var keyValuePair in toClean)
                {
                    Tuple<DateTime, Lazy<TValue>> pair;
                    _cache.TryRemove(keyValuePair.Key, out pair);

                    //if (pair.Item2.IsValueCreated)
                    {
                        _disposeBehavior(keyValuePair.Key, pair.Item2.Value);
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

                var toClean = _cache.Where(keyValuePair => _lastClean - keyValuePair.Value.Item1 >= TimeToClean);
                foreach (var keyValuePair in toClean)
                {
                    Tuple<DateTime, Lazy<TValue>> pair;
                    _cache.TryRemove(keyValuePair.Key, out pair);

                    //if (pair.Item2.IsValueCreated)
                    {
                        _disposeBehavior(keyValuePair.Key, pair.Item2.Value);
                    }
                }

                _lastClean = DateTime.Now;
            }
        }
        #endregion
    }
}
