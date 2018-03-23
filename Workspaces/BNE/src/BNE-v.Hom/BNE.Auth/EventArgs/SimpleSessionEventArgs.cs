using System.Web;

namespace BNE.Auth.EventArgs
{
    public class SimpleSessionEventArgs : System.EventArgs
    {
        public HttpSessionStateBase Session { get; set; }
    }

}
