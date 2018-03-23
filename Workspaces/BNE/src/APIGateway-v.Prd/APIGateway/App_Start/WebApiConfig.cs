using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Routing;
using BNE.Core.ExtensionsMethods;
using System.Web.Http.Routing;
using System.Web.Http.Cors;
using APIGateway.Model;
using APIGateway.Handlers;
using System.Net.Http;

namespace APIGateway
{
    public static class WebApiConfig
    {

        public static void Register(HttpConfiguration config)
        {
            SwaggerHandler swaggerHandler = new SwaggerHandler(config);
            ProxyHandler proxyHandler = new ProxyHandler(config);
            NoActionHandler noActionHandler = new NoActionHandler(config);
            
            // Web API configuration and services
            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);

            // Web API routes
            config.MapHttpAttributeRoutes();

            //Registrando todas as rotas para utilizar o própio framework no reconhecimento do Endpoint
            foreach (var api in Domain.Api.Listar())
            {
                //Registering endpoint to OAuth Token Generation
                if (api.AuthenticationType != null && api.AuthenticationType.OAuthConfig != null)
                {
                    OAuthConfig oAuthConfig = api.AuthenticationType.OAuthConfig;
                    IHttpRoute route = config.Routes.CreateRoute(Flurl.Url.Combine(api.UrlSuffix, oAuthConfig.TokenEndpoint),
                        new Dictionary<string, object> { { "controller", "OAuth" }, { "action", "Token" } },
                        new Dictionary<string, object>(),
                        new Dictionary<string, object> { { "TokenEndpoint", true},
                                                         { "SecretKey", oAuthConfig.SecretKey }, 
                                                         { "UrlSuffix", api.UrlSuffix},
                                                         { "BaseUrl", api.Url },
                                                         { "AuthenticationEndpoint", oAuthConfig.AuthenticationEndpoint },
                                                         { "Expiration", oAuthConfig.Expiration } }, noActionHandler);

                    config.Routes.Add("Token_Endpoint_" + api.UrlSuffix, route);
                }

                //Registering endpoints for swagger
                if (api.SwaggerConfig != null)
                {
                    String uiPath = Flurl.Url.Combine(api.UrlSuffix, api.SwaggerConfig.UIUrl) + "/{*path}";
                    String filePath = Flurl.Url.Combine(api.UrlSuffix, api.SwaggerConfig.FileUrl);

                    //Swagger File Route
                    IHttpRoute route = config.Routes.CreateRoute(filePath,
                        new Dictionary<string, object> { { "controller", "Swagger" }, { "action", "GetFile" } },
                        new Dictionary<string, object>(),
                        new Dictionary<string, object> { { "SwaggerFileEndpoint", true},
                                                         { "FileName", api.SwaggerConfig.FileName} }, noActionHandler);

                    config.Routes.Add("SwaggerFile_Endpoint_" + api.UrlSuffix, route);

                    //Swagger Ui Route
                    //Mapeando rota para o Handler do swagger
                    config.Routes.MapHttpRoute(
                        name: "SwaggerUi_Endpoint_" + api.UrlSuffix,
                        routeTemplate: uiPath,
                        defaults: new { controller = "Swagger", action = "UI" },
                        constraints: null,
                        handler: swaggerHandler
                    );
                }

                foreach (var endpoint in api.Endpoints)
                {
                    HttpMethod method = HttpMethod.Get;
                    switch (endpoint.MethodString)
                    {
                        case "GET":
                            method = HttpMethod.Get; break;
                        case "PUT":
                            method = HttpMethod.Put; break;
                        case "POST":
                            method = HttpMethod.Post; break;
                        case "DELETE":
                            method = HttpMethod.Delete; break;
                        case "HEAD":
                            method = HttpMethod.Head; break;
                        case "OPTIONS":
                            method = HttpMethod.Options; break;
                        case "TRACE":
                            method = HttpMethod.Trace; break;
                        default:
                            break;
                    }


                    IHttpRoute route = config.Routes.CreateRoute(
                        Flurl.Url.Combine(api.UrlSuffix, endpoint.RelativePath).Replace("%7B","{").Replace("%7D", "}"),
                        new Dictionary<string, object> { { "controller", "Proxy" }, { "action", "Forward" } },
                        new Dictionary<string, object> { { "httpMethod", new System.Web.Http.Routing.HttpMethodConstraint(method) } },
                        new Dictionary<string, object> { { "UrlSuffix", api.UrlSuffix }, { "EndpointId", endpoint.Id } }, proxyHandler);

                    config.Routes.Add("Endpoint_" + endpoint.Id.ToString(), route);
                }
            }

            //Mapeando rotas para admin
            config.Routes.MapHttpRoute(
                name: "AdminApis",
                routeTemplate: "admin/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional },
                constraints: null,
                handler: noActionHandler
            );

            //Mapeando rotas para admin
            config.Routes.MapHttpRoute(
                name: "AdminApis_2",
                routeTemplate: "admin/{controller}/{action}/{id}",
                defaults: null,
                constraints: null,
                handler: noActionHandler
            );

            //Mapeando rotas para admin
            //config.Routes.MapHttpRoute(
            //    name: "AdminApis_2",
            //    routeTemplate: "admin/estatisticas/{action}",
            //    defaults: null,
            //    constraints: null,
            //    handler: noActionHandler
            //);
            
            //Mapeando rotas diferenciadas para admin
            config.Routes.MapHttpRoute(
                name: "AdminApis_E01",
                routeTemplate: "admin/{controller}/{apiUrlSuffix}/{reverse}",
                defaults: new { reverse = RouteParameter.Optional },
                constraints: null,
                handler: noActionHandler
            );
        }
    }
}
