using BNE.Cache;

namespace BNE.Package.Cache
{
    public sealed class CachingServiceProvider
    {
        static readonly ICachingService instance = new RuntimeCachingService();

        public static ICachingService Instance
        {
            get
            {
                return instance;
            }
        }
    }
}
