using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BNE.PessoaFisica.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("Index", "PreCurriculo");
            //return RedirectToAction("Index", "PessoaFisica");
        }

        [OutputCache(Duration = 24 * 60, VaryByParam = "none")]
        public ActionResult Parametros()
        {
            return PartialView("_Parametros", new Models.Parametros());
        }
    }
}