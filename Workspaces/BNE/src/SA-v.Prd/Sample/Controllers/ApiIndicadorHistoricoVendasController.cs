using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Data.SqlClient;
using AdminLTE_Application.Models;

namespace AdminLTE_Application.Controllers
{
    public class ApiIndicadorHistoricoVendasController : ApiController
    {
        private Model db = new Model();

        // GET: api/Empresa
        public async Task<IHttpActionResult> GetVWEmpresas()
        {
            using (var context = new Model())
            {
                context.Database.CommandTimeout = 60;
                var result = context.Database
                    .SqlQuery<HistoricoVendas>("dbo.getindicadorhistoricovendas")
                    .ToList();

                return this.Ok(result);
            }
        }


        // GET: Empresa/ApiEmpresa/Indices/5
       [HttpGet]
       [ResponseType(typeof(VWEmpresa))]
       [Route("consulta/ApiEmpresa/{id:string}")]
        public async Task<IHttpActionResult> Indices(decimal id)
       {
           decimal cpf = Convert.ToDecimal(id);            

           using (var context = new Model())
           {
                context.Database.CommandTimeout = 60;
                var filial = new SqlParameter("@cpf", cpf);

               var result = context.Database
                   .SqlQuery<HistoricoVendas>("dbo.getindicadorhistoricovendas @cpf", filial)
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