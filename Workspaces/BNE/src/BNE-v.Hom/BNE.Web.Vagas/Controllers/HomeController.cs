using System.Web.Mvc;
using BNE.BLL.Custom;

namespace BNE.Web.Vagas.Controllers
{
    public class HomeController : BaseController
    {
        //
        // GET: /Home/
        [OutputCache(CacheProfile = "CacheUmDia")]
        public ActionResult Index()
        {
            var url = string.Concat("http://", Helper.RecuperarURLAmbiente());
            return Redirect(url);
        }

    }
}
