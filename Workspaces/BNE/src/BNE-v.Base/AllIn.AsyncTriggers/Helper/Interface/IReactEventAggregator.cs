using System;
namespace AllInTriggers.Helper
{
    public class ShootResultArgs<TEventArgs>
    {
        public ShootResultArgs(TEventArgs original, bool raised)
        {
            this.OriginalEvent = original;
            this.Raised = false;
        }

        public ShootResultArgs(TEventArgs original, Exception ex)
        {
            this.OriginalEvent = original;
            this.Raised = true;
            this.Exception = ex;
        }

        public bool Error
        {
            get
            {
                return Exception != null;
            }
        }

        public Exception Exception { get; protected set; }

        public TEventArgs OriginalEvent { get; protected set; }

        public bool Raised { get; protected set; }
    }

    public interface IReactRoutedEvent<TEventArgs, in TTargetRoute> : IReactEvent<TEventArgs>, IReactBroadcastEvent<TEventArgs>
    {
        IObservable<TEventArgs> Observe(TTargetRoute routedValue);
    }

    public interface IReactEvent<TEventArgs> : IEventObservable<TEventArgs>, IEventObserver<TEventArgs>
    {

    }

    public interface IEventObserver<in TEventArgs>
    {
        void Fire(TEventArgs args);
    }

    public interface IEventObservable<out TEventArgs>
    {
        IObservable<TEventArgs> Observe();
    }

}
