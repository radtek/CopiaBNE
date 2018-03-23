using System.Web.Mvc;
using System.Web.Routing;

namespace BNE.PessoaJuridica.Web
{
    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            RouteTable.Routes.AppendTrailingSlash = true;
            routes.IgnoreRoute("{resource}.caprf");
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

#if DEBUG
            routes.MapRoute(
                name: "Usuario",
                url: "Usuario/{email}/{numeroCNPJ}",
                defaults: new { controller = "Usuario", action = "Index", },
                constraints: new { email = @"^[\w\.=-]+@[\w\.-]+\.[\w]{2,3}$" }
            );

            routes.MapRoute(
                name: "IndexNomeEmailCNPJ",
                url: "Empresa/{nome}/{email}/{cnpj}",
                defaults: new { controller = "Empresa", action = "IndexNomeEmailCNPJ" },
                constraints: new
                {
                    email = @"^[\w\.=-]+@[\w\.-]+\.[\w]{2,3}$"
                }
            );

            routes.MapRoute(
                name: "IndexEmail",
                url: "Empresa/{email}",
                defaults: new { controller = "Empresa", action = "IndexEmail" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
#else
            routes.MapRoute(
                name: "Usuario",
                url: "pessoajuridica/Usuario/{email}/{numeroCNPJ}",
                defaults: new { controller = "Usuario", action = "Index", },
                constraints: new { email = @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" }
            );

            routes.MapRoute(
                name: "IndexNomeEmailCNPJ",
                url: "pessoajuridica/Empresa/{nome}/{email}/{cnpj}",
                defaults: new { controller = "Empresa", action = "IndexNomeEmailCNPJ", },
                constraints: new
                {
                    email = @"^[\w\.=-]+@[\w\.-]+\.[\w]{2,3}$"
                }
            );

            routes.MapRoute(
                name: "IndexEmail",
                url: "pessoajuridica/Empresa/{email}",
                defaults: new { controller = "Empresa", action = "IndexEmail", },
                constraints: new
                {
                    email = @"^[\w\.=-]+@[\w\.-]+\.[\w]{2,3}$"
                }
            );

            routes.MapRoute(
                name: "Default",
                url: "pessoajuridica/{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
#endif
        }

    }
}
