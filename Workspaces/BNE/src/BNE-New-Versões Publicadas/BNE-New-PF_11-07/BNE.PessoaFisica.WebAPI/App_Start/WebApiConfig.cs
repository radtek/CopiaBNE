using System.Configuration;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Routing;

namespace BNE.PessoaFisica.WebAPI
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
                routeTemplate: "api/pessoafisica/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional, action = RouteParameter.Optional }
            );

            //Config.Routes.MapHttpRoute(
            //    name: "DefaultApiAction",
            //    routeTemplate: "api/pessoafisica/{controller}/{action}"
            //);

            config.Routes.MapHttpRoute("DefaultApiWithAction1", "api/pessoafisica/{controller}/{action}", new { httpMethod = new HttpMethodConstraint(HttpMethod.Get) });
        }
    }
}
