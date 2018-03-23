using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace API.Gateway.Console.Controllers
{
    public class BNEController : Controller
    {

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ViewBag.ActiveUserKey = (filterContext.HttpContext.Session.Contents["active_user_key"] == null) ? "" : filterContext.HttpContext.Session.Contents["active_user_key"];
            base.OnActionExecuting(filterContext);
        }
    }
}