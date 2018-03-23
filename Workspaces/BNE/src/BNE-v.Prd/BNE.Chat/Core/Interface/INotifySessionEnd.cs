using BNE.Chat.Core.EventModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.SessionState;

namespace BNE.Chat.Core.Interface
{

    public interface INotifySessionEnd
    {
        event EventHandler<CurrentSessionEventArgs> ReceiveSessionEnd;
    }

}
