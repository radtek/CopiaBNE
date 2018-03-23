using System;
using System.Threading.Tasks;
using BNE.Chat.Core.Hubs;
using Microsoft.AspNet.SignalR;

namespace BNE.Chat.Core.EventModel
{
    public class ChatDefaultEventArgs : ChatEventArgs, IResultChatEventArgs
    {
        public ChatDefaultEventArgs(Hub hub, object chatArgs)
            : base(hub, chatArgs)
        {
        }

        public Task TaskResult { get; set; }
    }
}