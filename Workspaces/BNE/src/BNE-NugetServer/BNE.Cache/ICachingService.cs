using System;

namespace BNE.Cache
{
    public interface ICachingService
    {
        T GetItem<T>(string key, Func<T> itemCallback, double timeToRetain = 60) where T : class;
        T GetItem<T>(string key, Func<T> itemCallback, TimeSpan timeToRetain) where T : class;
        bool AddItem<T>(string key, T item, TimeSpan timeToRetain) where T : class;
        T GetItem<T>(string key) where T : class;
        void RemoveItem(string key);
    }
}
