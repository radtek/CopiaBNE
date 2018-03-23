using AdminLTE_Application;
using Sample.Models.Empresa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Sample.Controllers
{
    [Authorize]
    public class EmpresaProspeccaoController : Controller
    {
        // GET: EmpresaProspeccao
        public ActionResult Index(GridProspect model)
        {
            Model db = new Model();
            var empresas = from e in db.VWTanqueEmpresas select e;
            model.lista = empresas.ToList();
            return View("~/Views/Prospeccao/Index.cshtml", model);
        }
    }
}