using System;

namespace BNE.Chat.Helper
{
    public interface ITimeCache<TKey, TValue>
    {
        TimeSpan TimeToClean { get; }
        bool TryGetValue(TKey keyId, out TValue result);
        TValue GetOrAdd(TKey keyId, Func<TKey, TValue> valueFactory);
        bool TryRemove(TKey keyId, out TValue value);
        TValue AddOrUpdate(TKey keyId, Func<TKey, TValue> valueFactory, Func<TKey, TValue, TValue> update);
        void Clear();
        void TryCleanUp();
    }
}