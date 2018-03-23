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
    public class ApiPesquisaController : ApiController
    {
        private Model db = new Model();

        // GET: Empresa/ApiEmpresa/Indices/5
       [HttpGet]
       [ResponseType(typeof(VWEmpresa))]
       [Route("consulta/ApiPesquisa/{id:string}")]
        public async Task<IHttpActionResult> Indices(string id)
       {
           string pesquisa = id.Replace("dott",".").Replace("arroba","@");            
           
           using (var context = new Model())
           {
               var search = new SqlParameter("@search", pesquisa);

               var result = context.Database
                   .SqlQuery<Empresas>("dbo.searchGeral @search", search)
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