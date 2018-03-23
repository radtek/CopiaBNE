using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using BNE.Chat.Core.EventModel;
using BNE.Chat.Core.Hubs;
using BNE.Chat.Core.Interface;
using Microsoft.AspNet.SignalR;

namespace BNE.Chat.Core
{
    public class CounterConsumer : IDisposable
    {
        private readonly IDisposable _sDisposable;

        private readonly IChatListener _manager;
        private readonly CounterStore _counter;

        public CounterConsumer(IChatListener manager, CounterStore counterStore)
        {
            if (manager == null)
                throw new NullReferenceException("manager");

            if (counterStore == null)
                throw new NullReferenceException("counterChat");

            this._manager = manager;
            this._counter = counterStore;
            var compDisp = new System.Reactive.Disposables.CompositeDisposable();

            var dispA =
                  this._manager.OnNewConnection().Do(a => ChangeCounter(a, true)).Do(NotifyChanges).Subscribe();
            var dispB =
                this._manager.OnCloseConnection().Do(a => ChangeCounter(a, false)).Do(NotifyChanges).Subscribe();

            compDisp.Add(dispA);
            compDisp.Add(dispB);

            this._sDisposable = compDisp;
        }

        private void ChangeCounter(EventPattern<ChatEmptyEventArgs> chatParams, bool toUp)
        {
            _counter.ChangeCounter(
                CounterStore.GetTransportType(chatParams.EventArgs.Hub.Context.QueryString["transport"]), toUp);
        }

        private void NotifyChanges(EventPattern<ChatEmptyEventArgs> obj)
        {
            var hub = GlobalHost.ConnectionManager.GetHubContext<CounterHub>();

            if (hub == null)
                return;

            hub.Clients.All.recalculateOnlineUsers(_counter.Total);
        }

        public void Dispose()
        {
            var disp = _sDisposable;
            try
            {
                if (disp != null)
                    disp.Dispose();
            }
            catch (Exception)
            {
            }

        }
    }
}
