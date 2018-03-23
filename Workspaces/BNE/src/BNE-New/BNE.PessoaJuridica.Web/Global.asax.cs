using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using BNE.PessoaJuridica.Web.Autofac;
using log4net;

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
            var logger = AutofacDependencyResolver.Current.ApplicationContainer.Resolve<ILog>();

            logger.Error(Server.GetLastError());
        }
    }
}