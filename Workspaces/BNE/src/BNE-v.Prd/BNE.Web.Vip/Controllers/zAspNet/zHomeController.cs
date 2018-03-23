using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace BNE.Web.Vip.Controllers
{

    public class zHomeController : BaseMVCController
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Default";
            return View("~/Views/zAspNet/zHome/Index.cshtml");
        }
    }
}
