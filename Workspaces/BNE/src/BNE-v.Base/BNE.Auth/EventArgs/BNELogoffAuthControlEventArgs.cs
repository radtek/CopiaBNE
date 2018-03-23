using BNE.Auth.HttpModules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;

namespace BNE.Auth.HttpModules
{
    public class BNELogoffAuthControlEventArgs : BNEAuthEventArgs
    {
        public BNELogoffAuthControlEventArgs()
        {
        }
        public BNELogoffAuthControlEventArgs(HttpSessionState session)
            : base(session)
        {
        }
        public BNELogoffAuthControlEventArgs(HttpSessionStateBase session)
            : base(session)
        {
        }

        public BNELogoffAuthControlEventArgs(HttpContext context)
            : base(context)
        {
        }

        public BNELogoffAuthControlEventArgs(HttpContextBase context)
            : base(context)
        {
        }


        public LogoffType LogoffType { get; set; }

    }
}
