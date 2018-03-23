using System;
using System.Web.Http;
using BNE.Logger.Interface;

namespace BNE.PessoaJuridica.WebAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            Bootstrapper.Run();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var logger = (ILogger)GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(ILogger));

            logger.Error(Server.GetLastError());
        }
    }
}
