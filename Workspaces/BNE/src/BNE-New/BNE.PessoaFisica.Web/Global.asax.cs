using System;
using BNE.PessoaFisica.Web.Models;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using log4net;

namespace BNE.PessoaFisica.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {

        protected void Application_Error(object sender, EventArgs e)
        {
            var logger = AutofacDependencyResolver.Current.ApplicationContainer.Resolve<ILog>();

            logger.Error(Server.GetLastError());
        }

        protected void Application_Start()
        {
            ApplicationConfig.Configure(Application);
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Autofac.AutofacConfiguration.Register();

            ModelBinders.Binders.Add(typeof(decimal), new DecimalModelBinder());
            ModelBinders.Binders.Add(typeof(decimal?), new DecimalModelBinder());
        }
    }
}
