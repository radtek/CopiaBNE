using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using AdminLTE_Application;
using System.Data.SqlClient;
using AdminLTE_Application.Models;

namespace AdminLTE_Application.Controllers
{
    public class ApiVendedorEmpresaNaCarteiraController : ApiController
    {
        private Model db = new Model();

        // GET: api/Empresa
        public async Task<IHttpActionResult> GetVWEmpresas()
        {
            using (var context = new Model())
            {
                var result = context.Database
                    .SqlQuery<EmpresaAlertas>("dbo.getUsuarioEmpresasNaCarteira")
                    .ToList();

                return this.Ok(result);
            }
        }


        // GET: Empresa/ApiEmpresa/Indices/5
       [HttpGet]
       [ResponseType(typeof(VWEmpresa))]
       [Route("consulta/ApiEmpresa/{id:string}")]
        public async Task<IHttpActionResult> Indices(string id)
       {
           string nome = id.Replace("dott",".").Replace("arroba","@");            
           
           using (var context = new Model())
           {
               var cpf = new SqlParameter("@nome", nome);

               var result = context.Database
                   .SqlQuery<EmpresaAlertas>("dbo.getUsuarioEmpresasNaCarteira @nome", cpf)
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