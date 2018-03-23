using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace BNE.Auth.HttpModules
{
    public class BNEAuthStartSessionControlArgs : BNEAuthEventArgs
    {
        public BNEAuthStartSessionControlArgs()
        {

        }

        public BNEAuthStartSessionControlArgs(HttpContext context)
            : base(context)
        {
        }
        public BNEAuthModule.SessionStartEventControlType EventType { get; set; }
    }
}
