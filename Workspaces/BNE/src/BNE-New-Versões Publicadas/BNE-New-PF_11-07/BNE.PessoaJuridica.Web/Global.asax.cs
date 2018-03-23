using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using BNE.Logger.Interface;
using BNE.PessoaJuridica.Web.Autofac;

namespace BNE.PessoaJuridica.Web
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            ApplicationConfig.Configure(Application);
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            AutofacConfiguration.Configure();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var logger = AutofacDependencyResolver.Current.ApplicationContainer.Resolve<ILogger>();

            logger.Error(Server.GetLastError());
        }

    }
}
