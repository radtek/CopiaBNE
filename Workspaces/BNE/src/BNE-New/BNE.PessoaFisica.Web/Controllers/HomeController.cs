using System.Web.Mvc;

namespace BNE.PessoaFisica.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("Index", "PreCurriculo");
        }

        [OutputCache(Duration = 24 * 60, VaryByParam = "none")]
        public ActionResult Parametros()
        {
            return PartialView("_Parametros", new Models.Parametros());
        }
    }
}