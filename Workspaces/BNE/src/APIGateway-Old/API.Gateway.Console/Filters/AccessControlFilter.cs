using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace API.Gateway.Console.Filters
{
    public class AccessControlFilter :  ActionFilterAttribute, IActionFilter
    {
        void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session.Contents["active_user_key"] == null)
                filterContext.Result = new RedirectResult("/Acesso/Entrar");

            this.OnActionExecuting(filterContext);
        }
    }
}