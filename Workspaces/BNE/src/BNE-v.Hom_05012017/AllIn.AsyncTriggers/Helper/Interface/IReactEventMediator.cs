using System;
namespace AllInTriggers.Helper
{
    public interface IReactMediatorInvoker<TKey>
    {
        void PublishIfExists<T>(TKey eventKey, T args);

    }
}
