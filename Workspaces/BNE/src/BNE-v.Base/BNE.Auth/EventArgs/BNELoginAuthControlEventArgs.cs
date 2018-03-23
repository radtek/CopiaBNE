using Microsoft.IdentityModel.Claims;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using BNE.Auth.Helper;

namespace BNE.Auth.HttpModules
{
    public class BNEAuthLoginControlEventArgs : BNEAuthEventArgs
    {
        public BNEAuthLoginControlEventArgs(ClaimsIdentity identity)
        {
            Identity = identity;
        }
        public BNEAuthLoginControlEventArgs(ClaimsIdentity identity, HttpSessionState session)
            : base(session)
        {
            Identity = identity;
        }
        public BNEAuthLoginControlEventArgs(ClaimsIdentity identity, HttpSessionStateBase session)
            : base(session)
        {
            Identity = identity;
        }

        public BNEAuthLoginControlEventArgs(ClaimsIdentity identity, HttpContext context)
            : base(context)
        {
            Identity = identity;
        }

        public BNEAuthLoginControlEventArgs(ClaimsIdentity identity, HttpContextBase context)
            : base(context)
        {
            Identity = identity;
        }


        public bool PersistentWay { get; set; }
    }
}
