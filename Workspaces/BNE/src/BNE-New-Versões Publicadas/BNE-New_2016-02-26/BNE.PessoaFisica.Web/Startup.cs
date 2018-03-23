using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(BNE.PessoaFisica.Web.Startup))]

namespace BNE.PessoaFisica.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Auth.NET45.BNELoginConfigOwin.Configure(app);
        }
    }
}
