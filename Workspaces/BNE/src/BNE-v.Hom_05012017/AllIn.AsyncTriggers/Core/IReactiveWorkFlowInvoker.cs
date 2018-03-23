using System.Reactive.Concurrency;
using AllInTriggers.Helper;

namespace AllInTriggers.Core
{
    public interface IReactiveFlowInvoker
    {
        IScheduler Context { get; }
        ReactSharedEventMediator<string> Mediator { get; }
        bool OwnerOfContext { get; }
    }
}
