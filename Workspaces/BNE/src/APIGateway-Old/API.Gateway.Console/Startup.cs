using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(API.Gateway.Console.Startup))]
namespace API.Gateway.Console
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            
        }
    }
}
