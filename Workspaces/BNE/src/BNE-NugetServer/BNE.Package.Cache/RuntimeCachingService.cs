using System;
using System.Runtime.Caching;
using BNE.Package.Cache;

namespace BNE.Cache
{
    class RuntimeCachingService : ICachingService
    {

        private readonly MemoryCache _cache;

        #region RuntimeCachingService
        public RuntimeCachingService()
        {
            _cache = MemoryCache.Default;
        }
        #endregion

        #region GetItem
        public T GetItem<T>(string key, Func<T> itemCallback, double timeToRetain) where T : class
        {
            var cacheItem = _cache.Get(key) as Lazy<T>;
            if (cacheItem == null)
            {
                var cacheItemPolicy = new CacheItemPolicy { SlidingExpiration = TimeSpan.FromMinutes(timeToRetain), Priority = CacheItemPriority.Default };

                // http://msdn.microsoft.com/en-us/library/dd642331(v=vs.110).aspx
                cacheItem = new Lazy<T>(itemCallback, System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);

                if (_cache.Add(key, cacheItem, cacheItemPolicy))
                {
                    return cacheItem.Value;
                }
            }
            return ((Lazy<T>)_cache.Get(key)).Value;
        }
        #endregion

    }
}
