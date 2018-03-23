using System.Configuration;
using System.Web.Mvc;
using System.Web.Routing;

namespace BNE.PessoaFisica.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            var pathRouteApp = ConfigurationManager.AppSettings["pathRoutes"];

            #region Vaga
            routes.MapRoute("VagaLogado", "vaga-de-emprego-na-area-{AreaBNE}-em-{Cidade}-{SiglaEstado}/{Funcao}/{Identificador}/{CodigoPesquisa}/{Cpf}/{DataNascimento}",
                new {controller = "PreCurriculo", action = "Index", CodigoPesquisa = UrlParameter.Optional}
                ).DataTokens["RouteName"] = "VagaLogado";

            routes.MapRoute("Vaga1", "vaga-de-emprego-na-area-{AreaBNE}-em-{Cidade}-{SiglaEstado}/{Funcao}/{Identificador}/{CodigoPesquisa}",
                new {controller = "PreCurriculo", action = "Index", CodigoPesquisa = UrlParameter.Optional}
                ).DataTokens["RouteName"] = "Vaga1";

            routes.MapRoute("Estagio", "estagio-para-vaga-de-emprego-na-area-{AreaBNE}-em-{Cidade}-{SiglaEstado}/{Funcao}/{Identificador}/{CodigoPesquisa}", 
                new {controller = "PreCurriculo", action = "Index", CodigoPesquisa = UrlParameter.Optional}
                ).DataTokens["RouteName"] = "Estagio";
            #endregion Vaga

            routes.MapRoute("Default", pathRouteApp + "{controller}/{action}/{id}", new {controller = "Home", action = "Index", id = UrlParameter.Optional}
                );
        }
    }
}