using System.Configuration;
using System.Web.Http;
using System.Web.Http.Cors;

namespace BNE.Mensagem.WebAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var origins = ConfigurationManager.AppSettings["origins"];
            config.EnableCors(new EnableCorsAttribute(origins, "*", "*"));

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "Email",
                routeTemplate: "email/{sistema}/{template}",
                defaults: new { controller = "Email" }
            );

            config.Routes.MapHttpRoute(
                name: "SMS",
                routeTemplate: "sms/{sistema}/{template}",
                defaults: new { controller = "SMS" }
            );

        }
    }
}
