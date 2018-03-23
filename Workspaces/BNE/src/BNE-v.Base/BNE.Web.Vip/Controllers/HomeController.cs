using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BNE.Web.Vip.Controllers
{
    [Authorize]
    public class HomeController : BaseMVCController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}