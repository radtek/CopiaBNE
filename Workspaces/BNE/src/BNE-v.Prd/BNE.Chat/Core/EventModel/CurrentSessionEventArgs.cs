using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace BNE.Chat.Core.EventModel
{
    public class CurrentSessionEventArgs : EventArgs
    {
        public HttpSessionStateBase Current { get; set; }
    }
}
