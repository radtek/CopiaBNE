using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(BNE.ExceptionLog.WebAPI.Startup))]

namespace BNE.ExceptionLog.WebAPI
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
