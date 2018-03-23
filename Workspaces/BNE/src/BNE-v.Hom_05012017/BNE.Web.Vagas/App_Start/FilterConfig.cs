using System.Web.Mvc;
using BNE.Web.Vagas.Code.ActionFilter;

namespace BNE.Web.Vagas
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            //filters.Add(new LoginActionFilterAttribute()); Login controlado pelos HTTPHandlers
        }
    }
}