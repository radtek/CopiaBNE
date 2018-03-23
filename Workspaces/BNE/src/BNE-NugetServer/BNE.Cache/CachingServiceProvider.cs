namespace BNE.Cache
{
    public sealed class CachingServiceProvider
    {
        public static ICachingService Instance { get; } = new RuntimeCachingService();
    }
}
