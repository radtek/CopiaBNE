using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
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

            routes.MapRoute(
                name: "VagaLogado",
                url: "vaga-de-emprego-na-area-{AreaBNE}-em-{Cidade}-{SiglaEstado}/{Funcao}/{Identificador}/{CodPesquisa}/{Cpf}/{DataNascimento}",
                defaults: new { controller = "PreCurriculo", action = "Index", CodPesquisa = UrlParameter.Optional }
            ).DataTokens["RouteName"] = "VagaLogado";

            routes.MapRoute(
                name: "Vaga1",
                url: "vaga-de-emprego-na-area-{AreaBNE}-em-{Cidade}-{SiglaEstado}/{Funcao}/{Identificador}/{CodPesquisa}",
                defaults: new { controller = "PreCurriculo", action = "Index", CodPesquisa = UrlParameter.Optional }
            ).DataTokens["RouteName"] = "Vaga1";

            routes.MapRoute(
                name: "Estagio",
                url: "estagio-para-vaga-de-emprego-na-area-{AreaBNE}-em-{Cidade}-{SiglaEstado}/{Funcao}/{Identificador}/{CodPesquisa}",
                defaults: new { controller = "PreCurriculo", action = "Index", CodPesquisa = UrlParameter.Optional }
            ).DataTokens["RouteName"] = "Vaga1";

            #endregion Vaga


            routes.MapRoute(
                name: "Default",
                url: pathRouteApp + "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            
        }
    }
}
