using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BNE.Web.Parceiros.Controllers.Autorizacao
{
    public class AutorizacaoActionFilter : ActionFilterAttribute, IActionFilter
    {

        void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
        {
            var rd = filterContext.RouteData;
            string currentAction = rd.GetRequiredString("action");
            string currentController = rd.GetRequiredString("controller");
            var currentRoute = string.Format("{0}/{1}", currentController, currentAction);


            if(currentRoute == "Login/Index" && filterContext.HttpContext.Session["ItemSession"] != null)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    action = "Index",
                    controller = "Perfil",
                    area = ""
                }));
            }
            else if (currentRoute == "Login/Entrar")
            {
                this.OnActionExecuting(filterContext);
            }
            else if (filterContext.HttpContext.Session["ItemSession"] == null && currentRoute != "Login/Entrar" && currentRoute != "Login/Index")
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    action = "Index",
                    controller = "Login",
                    area = ""
                }));
            }

            
        }
    }
}