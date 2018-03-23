using System;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using BNE.Chat.Core.EventModel;
using Microsoft.AspNet.SignalR.Hubs;

namespace BNE.Chat.Core.Hubs
{
    [HubName("hitCounterServer")]
    public class CounterHub : Microsoft.AspNet.SignalR.Hub
    {
        private readonly CounterStore _counter;

        public CounterHub(CounterStore counter)
        {
            if (counter == null)
            {
                Trace.WriteLine("Warning! 'CounterHub' will not work correctly, there is a invalid 'CounterStore'.");
                return;
            }
            this._counter = counter;
        }

        public int GetTotalOnline()
        {
            if (_counter == null)
                return -1;

            return _counter.Total;
        }
    }
}
