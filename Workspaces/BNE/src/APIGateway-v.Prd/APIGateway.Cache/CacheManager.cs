using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace APIGateway.Cache
{
    public class CacheManager
    {
        private static ObjectCache _cache = MemoryCache.Default;

        public static void Cache(string CacheKeyName, Object obj)
        {
            var policy = new CacheItemPolicy();
            policy.Priority = CacheItemPriority.Default;
            policy.AbsoluteExpiration = DateTimeOffset.Now.AddHours(24);
            _cache.Set(CacheKeyName, obj, policy);
        }

        public static void Uncache(string CacheKeyName)
        {
            if (_cache.Contains(CacheKeyName))
            {
                _cache.Remove(CacheKeyName);
            }
        }

        public static Object GetCached(string CacheKeyName)
        {
            return _cache[CacheKeyName];
        }
    }
}
