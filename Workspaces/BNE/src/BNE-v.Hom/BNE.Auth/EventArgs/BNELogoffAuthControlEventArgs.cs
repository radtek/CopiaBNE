using System.Web;
using System.Web.SessionState;
using BNE.Auth.Core.Enumeradores;

namespace BNE.Auth.EventArgs
{
    public class BNELogoffAuthControlEventArgs : BNEAuthEventArgs
    {
        public BNELogoffAuthControlEventArgs() { }
        public BNELogoffAuthControlEventArgs(HttpSessionState session) : base(session) { }
        public BNELogoffAuthControlEventArgs(HttpSessionStateBase session) : base(session) { }
        public BNELogoffAuthControlEventArgs(HttpContext context) : base(context) { }
        public BNELogoffAuthControlEventArgs(HttpContextBase context) : base(context) { }
        public LogoffType LogoffType { get; set; }
    }
}
