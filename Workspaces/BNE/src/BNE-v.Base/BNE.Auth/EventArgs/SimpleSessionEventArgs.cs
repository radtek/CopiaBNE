using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace BNE.Auth
{
    public class SimpleSessionEventArgs : EventArgs
    {
        public HttpSessionStateBase Session { get; set; }
    }

}
