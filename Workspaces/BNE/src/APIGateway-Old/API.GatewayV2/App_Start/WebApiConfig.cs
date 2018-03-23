using API.GatewayV2.APIModel;
using API.GatewayV2.Throttle;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Cors;
using System.Web.Http;
using System.Web.Http.Cors;
using WebApiThrottle;

namespace API.GatewayV2
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // WebApiConfig.cs
            var corsAttr = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(corsAttr);

            config.Routes.MapHttpRoute(
               name: "DefaultApi",
               routeTemplate: "{api}/{VersionAPI}/{RemoteController}/{RemoteAction}",
               defaults: new { controller = "Proxy" }
           );

            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();


            CustomThrottlingHandler throttlingHandler = new CustomThrottlingHandler();
            throttlingHandler.Policy = new ThrottlePolicy()
            {
                ClientThrottling = true
            };

            throttlingHandler.Policy.ClientRules = new Dictionary<string, RateLimits>();
            throttlingHandler.QuotaExceededResponseCode = HttpStatusCode.ServiceUnavailable;
            throttlingHandler.QuotaExceededMessage = "Limites de requisição excedida! Máximo permitido {0} por {1}.";


            throttlingHandler.PolicyRepository = new PolicyMemoryCacheRepository();
            throttlingHandler.Repository = new MemoryCacheRepository();
            throttlingHandler.PolicyRepository.Save(ThrottleManager.GetPolicyKey(), throttlingHandler.Policy);
            config.MessageHandlers.Add(throttlingHandler);

        }
    }
}
