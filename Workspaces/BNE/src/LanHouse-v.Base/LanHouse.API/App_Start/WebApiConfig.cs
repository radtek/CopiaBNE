using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Routing;

namespace LanHouse.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Web API routes
            config.MapHttpAttributeRoutes();

            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            config.Formatters.Add(new BsonMediaTypeFormatter());
            config.Routes.MapHttpRoute("DefaultApi1", "Api/{controller}");
            config.Routes.MapHttpRoute("DefaultApiGetID1", "Api/{controller}/{action}/{id}");
            config.Routes.MapHttpRoute("DefaultApiPostID1", "Api/{controller}/{action}/{id}");
            config.Routes.MapHttpRoute("DefaultApiWithAction", "Api/{controller}/{action}", new { action="Options"}, new { httpMethod = new HttpMethodConstraint(HttpMethod.Options) });

            config.Routes.MapHttpRoute("DefaultApiWithId", "Api/{controller}/{id}", new { id = RouteParameter.Optional }, new { id = @"\d+" });
            config.Routes.MapHttpRoute("DefaultApiWithAction1", "Api/{controller}/{action}", new { httpMethod = new HttpMethodConstraint(HttpMethod.Get) });
            config.Routes.MapHttpRoute("DefaultApiGet", "Api/{controller}", new { action = "Get" }, new { httpMethod = new HttpMethodConstraint(HttpMethod.Get) });
            config.Routes.MapHttpRoute("DefaultApiPost", "Api/{controller}", new { action = "Post" }, new { httpMethod = new HttpMethodConstraint(HttpMethod.Post) });
            config.Routes.MapHttpRoute("DefaultApiGetID", "Api/{controller}/{action}/{id}", new { action = "Get" }, new { httpMethod = new HttpMethodConstraint(HttpMethod.Get) });
            config.Routes.MapHttpRoute("DefaultApiPostID", "Api/{controller}/{action}/{id}", new { action = "Post" }, new { httpMethod = new HttpMethodConstraint(HttpMethod.Post) });

        }
    }
}
