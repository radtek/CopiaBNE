using System;
using System.Runtime.Caching;
using System.Threading;

namespace BNE.Cache
{
    public class RuntimeCachingService : ICachingService
    {
        private readonly MemoryCache _cache;
        public RuntimeCachingService()
        {
            _cache = MemoryCache.Default;
        }
        public void RemoveItem(string key)
        {
            if (_cache.Contains(key))
            {
                _cache.Remove(key);
            }
        }
        public T GetItem<T>(string key, Func<T> itemCallback, double timeToRetain = 60) where T : class
        {
            return GetItem(key, itemCallback, TimeSpan.FromMinutes(timeToRetain));
        }
        public T GetItem<T>(string key, Func<T> itemCallback, TimeSpan timeToRetain) where T : class
        {
            var cacheItem = _cache.Get(key) as Lazy<T>;
            if (cacheItem == null)
            {
                var cacheItemPolicy = new CacheItemPolicy { AbsoluteExpiration = new DateTimeOffset(DateTime.Now.Add(timeToRetain)), Priority = CacheItemPriority.Default };

                // http://msdn.microsoft.com/en-us/library/dd642331(v=vs.110).aspx
                cacheItem = new Lazy<T>(itemCallback, LazyThreadSafetyMode.ExecutionAndPublication);

                if (_cache.Add(key, cacheItem, cacheItemPolicy))
                {
                    return cacheItem.Value;
                }
            }
            return ((Lazy<T>)_cache.Get(key)).Value;
        }
        public bool AddItem<T>(string key, T item, TimeSpan timeToRetain) where T : class
        {
            var cacheItemPolicy = new CacheItemPolicy { AbsoluteExpiration = new DateTimeOffset(DateTime.Now.Add(timeToRetain)), Priority = CacheItemPriority.Default };

            return _cache.Add(key, item, cacheItemPolicy);
        }
        public T GetItem<T>(string key) where T : class
        {
            return (T)_cache.Get(key);
        }
    }
}