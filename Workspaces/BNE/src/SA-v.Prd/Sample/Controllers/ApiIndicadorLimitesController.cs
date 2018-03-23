using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Data.SqlClient;
using AdminLTE_Application.Models;

namespace AdminLTE_Application.Controllers
{
    public class ApiIndicadorLimitesController : ApiController
    {
        private Model db = new Model();

        // GET: api/Empresa
        public IQueryable<VWEmpresa> GetVWEmpresas()
        {
            return db.VWEmpresas;
        }


        // GET: Empresa/ApiEmpresa/Indices/5
       [HttpGet]
       [ResponseType(typeof(VWEmpresa))]
       [Route("consulta/ApiEmpresa/{id:string}")]
        public async Task<IHttpActionResult> Indices(decimal id)
       {
           decimal Num_CNPJ = Convert.ToDecimal(id);            

           using (var context = new Model())
           {
               var filial = new SqlParameter("@num_cnpj", Num_CNPJ);

               var result = context.Database
                   .SqlQuery<Limites>("dbo.getIndicadoresSaldoDisponivelEmpresa @num_cnpj", filial)
                   .ToList();

               return this.Ok(result);
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

        private bool VWEmpresaExists(string id)
        {
            return db.VWEmpresas.Count(e => e.Raz_Social == id) > 0;
        }
    }
}