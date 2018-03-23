using API.Gateway.Console.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace API.Gateway.Console.Controllers
{
    [AccessControlFilter]
    public class WebapiController : BNEController
    {
        // GET: Webapi
        public ActionResult Index()
        {
            return View();
        }
    }
}