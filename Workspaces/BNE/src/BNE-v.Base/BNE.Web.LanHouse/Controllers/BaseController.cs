using System;
using System.Web.Mvc;

namespace BNE.Web.LanHouse.Controllers
{
    public class BaseController : Controller
    {

        #region JsonResult RecuperarDataAtual
        public JsonResult RecuperarDataAtual()
        {
            return Json(DateTime.Now.ToString("dd/MM/yyyy"), JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}