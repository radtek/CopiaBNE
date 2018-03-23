using BNE.Web.Parceiros.Controllers.Autorizacao;
using System.Web;
using System.Web.Mvc;

namespace BNE.Web.Parceiros
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new AutorizacaoActionFilter());
        }
    }
}