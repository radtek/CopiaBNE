using System;
using System.Web;
using System.Web.Http;
using BNE.Logger.Interface;

namespace BNE.PessoaFisica.WebAPI
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            Bootstrapper.Run();
            //GlobalConfiguration.Configuration.MessageHandlers.Add(new Handlers.AuthenticationHandler());
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var logger = (ILogger)GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(ILogger));
         
            logger.Error(Server.GetLastError());
        }
    }
}