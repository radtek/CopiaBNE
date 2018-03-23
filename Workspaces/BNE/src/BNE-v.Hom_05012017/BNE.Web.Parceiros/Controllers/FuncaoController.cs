using BNE.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BNE.Web.Parceiros.Controllers
{
    public class FuncaoController : Controller
    {

        public JsonResult Pesquisar(string term)
        {
            string[] funcoes = {};
            try   { funcoes = Funcao.RecuperarFuncoes(term, 20, 1); }
            catch { }
            return Json(funcoes);
        }

    }
}
