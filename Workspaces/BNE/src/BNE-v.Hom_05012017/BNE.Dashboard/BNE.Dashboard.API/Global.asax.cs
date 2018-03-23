using System.Web.Http;

namespace BNE.Dashboard.API
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configuration.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
            GlobalConfiguration.Configure(WebApiConfig.Register);
            Bootstrapper.Configure();
        }
    }
}
