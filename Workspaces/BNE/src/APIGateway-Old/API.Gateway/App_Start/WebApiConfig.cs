using API.Gateway.Models;
using API.Gateway.Throttle;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Formatting;
using System.Web.Http;
using WebApiThrottle;

namespace API.Gateway
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
               name: "DefaultApi",
               routeTemplate: "api/{VersionAPI}/{api}/{RemoteController}/{RemoteAction}",
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

            //Regra padrão
            using (var dbo = new APIGatewayContext())
            {
                var data_set = dbo.Usuario.Join(dbo.Quota.Include("Endpoint"), q => q.Idf_Perfil, pu => pu.Idf_Perfil, (pu, q) => new { Usuario = pu, Quota = q });
                foreach (var pq in data_set)
                {
                    string quota_rota_perfil = string.Format("{0}/api/{1}/{2}/{3}/{4}", pq.Usuario.Idf_Usuario, pq.Quota.Endpoint.VersionAPI,
                        pq.Quota.Endpoint.Nme_Api, pq.Quota.Endpoint.Controller, pq.Quota.Endpoint.Action);
                    throttlingHandler.Policy.ClientRules.Add(quota_rota_perfil, new RateLimits()
                    {
                        PerSecond = pq.Quota.Per_Second,
                        PerMinute = pq.Quota.Per_Minute,
                        PerHour = pq.Quota.Per_Hour,
                        PerDay = pq.Quota.Per_Day,
                        PerWeek = 0 //Controlado pelo ProxyController
                    });
                }
            }

            throttlingHandler.QuotaExceededResponseCode = HttpStatusCode.ServiceUnavailable;
            throttlingHandler.QuotaExceededMessage = "Limites de requisição excedida! Máximo permitido {0} por {1}.";


            throttlingHandler.PolicyRepository = new PolicyMemoryCacheRepository();
            throttlingHandler.Repository = new MemoryCacheRepository();

            //----------------------------
            throttlingHandler.PolicyRepository.Save(ThrottleManager.GetPolicyKey(), throttlingHandler.Policy);
            //-------------------------
            config.MessageHandlers.Add(throttlingHandler);

        }
    }
}
