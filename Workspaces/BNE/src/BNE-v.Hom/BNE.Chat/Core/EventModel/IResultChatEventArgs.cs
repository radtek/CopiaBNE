using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BNE.Chat.Core.EventModel
{
    public interface IResultChatEventArgs
    {
        Task TaskResult { get; set; }
    }

}
