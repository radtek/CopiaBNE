using System;
using BNE.Chat.Core.Hubs;
using Microsoft.AspNet.SignalR;

namespace BNE.Chat.Core.EventModel
{
    public abstract class BasicChatEventArgs : EventArgs
    {
        private readonly Hub _hub;
        protected BasicChatEventArgs(Hub hub)
        {
            if (hub == null)
                throw new NullReferenceException("hub");

            this._hub = hub;
        }

        public Hub Hub
        {
            get { return _hub; }
        }
    }

   
}