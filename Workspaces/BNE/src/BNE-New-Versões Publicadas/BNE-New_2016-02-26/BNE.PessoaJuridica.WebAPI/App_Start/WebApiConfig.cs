using System.Configuration;
using System.Web.Http;
using System.Web.Http.Cors;

namespace BNE.PessoaJuridica.WebAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var origins = ConfigurationManager.AppSettings["origins"];
            config.EnableCors(new EnableCorsAttribute(origins, "*", "*"));

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApiAction",
                routeTemplate: "api/pessoajuridica/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/pessoajuridica/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
