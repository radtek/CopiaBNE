using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using AdminLTE_Application.Models;
using System.Data.SqlClient;
using System.Security.Claims;
using Sample.DTO;
using Sample.Models.Empresa;
using AdminLTE_Application;
using Sample.Models;
using System.Collections.Generic;
using Sample.BLL;
using System.Web;
using Newtonsoft.Json;

namespace Sample.Controllers
{
    public class CriarCampanhaController : Controller
    {
        private Model db = new Model();
        // GET: CriarCampanha
        public ActionResult Index(CampanhaModel model) {
            model.ListaEmpresa = new System.Collections.Generic.List<CampanhaEmpresaModel>();
            model.cnpjs =  model.cnpjs.Remove(model.cnpjs.LastIndexOf(','));
            var lista = model.cnpjs.Split(',');
        
            using (var context = new Model())
            {
                var listEmpresa = (from q in db.VWEmpresas where lista.Contains(q.Num_CNPJ.ToString())  select q).ToList();
                foreach (var item in listEmpresa)
                {
                    model.ListaEmpresa.Add(new CampanhaEmpresaModel { num_cnpj = item.Num_CNPJ, Raz_Social = item.Raz_Social, Nme_Usuario = item.Nme_Usuario, Eml_Filial = item.Eml_Filial });
                }
            }
          model.cnjpsJson = JsonConvert.SerializeObject(model.ListaEmpresa);

            return View("~/Views/CampanhasEmpresa/CriarCampanha.cshtml", model);
        }

       

    }
}