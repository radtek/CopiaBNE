using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BNE.Web.Vip
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            BNE.Auth.NET45.BNELoginConfigOwin.Configure(app);
            app.MapSignalR();
        }
    }
}