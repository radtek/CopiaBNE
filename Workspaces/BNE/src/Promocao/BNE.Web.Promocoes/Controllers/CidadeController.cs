using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BNE.Web.Promocoes.Controllers
{
    public class CidadeController : Controller
    {
        //
        // GET: /Cidade/
        public JsonResult List(String desc)
        {
            try
            {
                var data = BLL.Cidade.RecuperarNomesCidadesEstado(desc, string.Empty, 10).ToDictionary(d => d.Key.ToString(), d => d.Value.ToString());
                List<String> cidades = new List<string>();
                foreach (var a in data)
                {
                    cidades.Add(a.Value);
                }
                return Json(cidades, JsonRequestBehavior.AllowGet);
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
                String[] cidade = desc.Split('/');
                if (cidade.Length > 1){
                    var data = BLL.Cidade.RecuperarNomesCidadesEstado(cidade[0],cidade[1], 1).ToDictionary(d => d.Key.ToString(), d => d.Value.ToString());
                    foreach(var a in data){
                        if (a.Value == desc.Trim())
                            return Json("True", JsonRequestBehavior.AllowGet);
                        else
                            return Json("", JsonRequestBehavior.AllowGet);
                        }
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