using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BNE.Chat.Core.EventModel
{
    public class ChatEmptyEventArgs : BasicChatEventArgs, IResultChatEventArgs
    {
        public ChatEmptyEventArgs(Hub hub) : base(hub)
        {
        }

        public Task TaskResult { get; set; }
    }
}
