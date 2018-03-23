using System.Web;
using BNE.Auth.HttpModules;

namespace BNE.Auth.EventArgs
{
    public class BNEAuthStartSessionControlArgs : BNEAuthEventArgs
    {
        public BNEAuthStartSessionControlArgs() { }
        public BNEAuthStartSessionControlArgs(HttpContext context) : base(context) { }
        public BNEAuthModule.SessionStartEventControlType EventType { get; set; }
    }
}
