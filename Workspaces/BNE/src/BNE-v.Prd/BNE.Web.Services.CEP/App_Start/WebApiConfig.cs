using System.Configuration;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace BNE.Web.Services.Solr
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var origins = ConfigurationManager.AppSettings["origins"];
            var cors = new EnableCorsAttribute(origins, "*", "*");
            config.EnableCors(cors);

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
