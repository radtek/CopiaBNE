using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using AdminLTE_Application;
using Sample.Models;
using System.Security.Claims;
using System.Data.SqlClient;

namespace Sample.Controllers
{
    public class VagaObservacaoController : Controller
    {
        private Model db = new Model();

        // GET: VagasEmpresa
        public async Task<ActionResult> Index(int idVaga,string s)
        {
            VagaObservacaoModel modelVagaObs = new VagaObservacaoModel();

            modelVagaObs.vaga = db.VWVaga.Where(x => x.Codigo.Equals(idVaga)).FirstOrDefault();
            modelVagaObs.listaObservacao = db.VWVagaObservacao.Where(x => x.Idf_Vaga.Equals(idVaga)).OrderByDescending(x => x.Dta_Cadastro);
            if (!string.IsNullOrEmpty(s) && s.Equals("1"))
                ViewBag.Msg = "sucesso";
            else if(!string.IsNullOrEmpty(s) && s.Equals("-1"))
                ViewBag.Msg = "erro";

            return View("~/Views/VagasEmpresa/vagaObservacao.cshtml", modelVagaObs);
        }

        public async Task<ActionResult> salvarObservacao(VagaObservacaoModel model)
        {
            var userWithClaims = (ClaimsPrincipal)User;
            var cpf = userWithClaims.Claims.First(c => c.Type == "cpf");

            using (var context = new Model())
            {
                var num_cpf = new SqlParameter("@num_cpf", cpf.Value);
                var Dsc_Observacao = new SqlParameter("@Des_Observacao", model.Observacao);
                var Idf_Vaga = new SqlParameter("@Idf_Vaga", model.vaga.Codigo);


                //if (!(dto.Msg == null))
                //{

                var result = context.Database.ExecuteSqlCommand("dbo.SetVagaObservacao @num_cpf, @Des_Observacao, @Idf_Vaga", num_cpf, Dsc_Observacao, Idf_Vaga);

                
                //}
                return Redirect("../VagaObservacao?idVaga=" + model.vaga.Codigo + "&s=" + result);
            }
           
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
