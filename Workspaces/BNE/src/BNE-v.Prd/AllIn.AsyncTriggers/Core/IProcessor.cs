using System;
using AllInTriggers.Core;

namespace AllInTriggers
{

    public interface IProcessor
    {
         IDisposable CreateSubscriptions(IReactiveFlowInvoker invoker);
    }

}
