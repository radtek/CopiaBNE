using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.SessionState;

namespace BNE.Web.Vip
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        public override void Init()
        {
            this.BeginRequest += MvcApplication_BeginRequest;
            this.EndRequest += MvcApplication_EndRequest;
            this.PostAuthorizeRequest += MvcApplication_PostAuthorizeRequest;

            base.Init();
        }

        void MvcApplication_EndRequest(object sender, System.EventArgs e)
        {

        }

        void MvcApplication_BeginRequest(object sender, System.EventArgs e)
        {
        }

        public static readonly Lazy<string> PathSignalR = new Lazy<string>(() => "/signalr");
        void MvcApplication_PostAuthorizeRequest(object sender, System.EventArgs e)
        {
            var app = ((HttpApplication)sender);
            if (app.Request.Path.Contains(PathSignalR.Value))
            {
                app.Context.SetSessionStateBehavior(SessionStateBehavior.ReadOnly);
            }
            else
            {
                if (WebApiConfig.IsWebApiRequest(app.Request.Path.ToString()))
                    System.Web.HttpContext.Current.SetSessionStateBehavior(System.Web.SessionState.SessionStateBehavior.Required);
            }
        }

    }
}
