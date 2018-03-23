using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(BNE.PessoaJuridica.Web.Startup))]

namespace BNE.PessoaJuridica.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //Configurando autenticação.
            Auth.NET45.BNELoginConfigOwin.Configure(app);
        }
    }
}
