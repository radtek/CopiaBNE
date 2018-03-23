using System.Web.Mvc;

namespace BNE.PessoaJuridica.Web.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Empresa");
        }

        [OutputCache(Duration = 24 * 60, VaryByParam = "none")]
        public ActionResult Parametros()
        {
            return PartialView("_Parametros", new Models.Parametros());
        }

    }
}