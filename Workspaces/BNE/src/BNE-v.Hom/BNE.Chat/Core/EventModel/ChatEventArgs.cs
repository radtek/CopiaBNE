using System;
using BNE.Chat.Core.Hubs;
using Microsoft.AspNet.SignalR;

namespace BNE.Chat.Core.EventModel
{
    public abstract class ChatEventArgs : BasicChatEventArgs
    {
        private readonly object _callParams;
  
        protected ChatEventArgs(Hub hub) :base(hub)
        {

        }

        protected ChatEventArgs(Hub hub, object callParams) : base(hub)
        {
            this._callParams = callParams;
        }

        public object CallParams
        {
            get { return _callParams; }
        }
    }
}