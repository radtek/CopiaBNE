using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using System.Web.Routing;

namespace BNE.Web.Vip
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            //config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Use camel case for JSON data.
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }

        public static bool IsWebApiRequest(string routeRequest)
        {
            if (routeRequest.IndexOf("api", StringComparison.OrdinalIgnoreCase) > -1)
                return true;
            else if (routeRequest.IndexOf("sair", StringComparison.OrdinalIgnoreCase) > -1 || routeRequest.IndexOf("logout", StringComparison.OrdinalIgnoreCase) > -1)
                return true;

            return false;
        }
    }
}
