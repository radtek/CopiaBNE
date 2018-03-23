using System;

namespace BNE.Package.Cache
{
    public interface ICachingService
    {
        T GetItem<T>(string key, Func<T> itemCallback, double timeToRetain) where T : class;
    }
}
