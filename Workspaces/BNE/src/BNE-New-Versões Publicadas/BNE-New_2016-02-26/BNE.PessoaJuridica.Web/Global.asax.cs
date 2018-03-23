using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
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

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            HttpApplication application = (HttpApplication)sender;
            HttpRequest request = application.Context.Request;
            request.InsertEntityBody();
        }

    }
}
