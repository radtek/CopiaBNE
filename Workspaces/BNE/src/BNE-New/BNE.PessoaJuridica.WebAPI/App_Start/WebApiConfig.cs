using System.Configuration;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Cors;

namespace BNE.PessoaJuridica.WebAPI
{
    public static class WebApiConfig
    {
        public static void Register(this HttpConfiguration config)
        {
            var origins = ConfigurationManager.AppSettings["origins"];
            config.EnableCors(new EnableCorsAttribute(origins, "*", "*"));

            config.MapHttpAttributeRoutes();

            config.Formatters.Clear();
            config.Formatters.Add(new JsonMediaTypeFormatter());
        }
    }
}
