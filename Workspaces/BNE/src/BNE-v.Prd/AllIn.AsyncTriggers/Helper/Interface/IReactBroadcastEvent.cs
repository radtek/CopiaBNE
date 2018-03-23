using System;
namespace AllInTriggers.Helper
{
    public interface IReactBroadcastEvent<TEventArgs>
    {
        IObservable<ShootResultArgs<TEventArgs>> Broadcast();
    }
}
