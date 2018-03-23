using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BNE.Web.Promocoes.Controllers
{
    public class FuncaoController : Controller
    {
        //
        // GET: /Funcao/

        public JsonResult List(String desc)
        {
            try
            {
                var data = BLL.Funcao.RecuperarFuncoes(desc, 10, null);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }


        }

        public JsonResult Val(String desc)
        {
            try
            {
                var data = BLL.Funcao.RecuperarFuncoes(desc, 1, null);
                foreach(var f in data){
                if (data.Count() == 1 && desc.Trim() == f.ToString() )
                    return Json("True", JsonRequestBehavior.AllowGet);
                else
                    return Json("", JsonRequestBehavior.AllowGet);
                }
                return Json("", JsonRequestBehavior.AllowGet);
               
            }
            catch (Exception ex)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }


        }

    }
}
