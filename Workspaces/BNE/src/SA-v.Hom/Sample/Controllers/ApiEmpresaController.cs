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
    public class ApiEmpresaController : ApiController
    {
        private Model db = new Model();

        // GET: api/Empresa
        public IQueryable<VWEmpresa> GetVWEmpresas()
        {
            return db.VWEmpresas;
        }

        // GET: api/Empresa/5
        [HttpGet]
        [ResponseType(typeof(VWEmpresa))]
        [Route("consulta/ApiEmpresa/{id:string}")]
        public async Task<IHttpActionResult> GetVWEmpresa(decimal id)
        {

            decimal Num_CNPJ = Convert.ToDecimal(id);
            
            var empresa = from q in db.VWEmpresas
                          where q.Num_CNPJ == Num_CNPJ
                            select new 
                            {
                                Raz_Social = q.Raz_Social,
                                Num_CNPJ = q.Num_CNPJ,
                                End_Site  = q.End_Site,
                                Num_DDD_Comercial = q.Num_DDD_Comercial,
                                Num_Comercial = q.Num_Comercial,
                                Des_Natureza_Juridica = q.Des_Natureza_Juridica,
                                Des_Plano = q.Des_Plano,
                                Dta_Inicio_Plano = q.Dta_Inicio_Plano,
                                Dta_Fim_plano = q.Dta_Fim_plano,
                                Sig_Estado = q.Sig_Estado,
                                Nme_Cidade = q.Nme_Cidade,
                                Des_Situacao_Filial = q.Des_Situacao_Filial,
                                Dta_Cadastro = q.Dta_Cadastro,
                                Des_URL = q.Des_URL,
                                Dta_Retrono = q.Dta_Retorno
                            };
            return this.Ok(empresa);

        }

       
        // PUT: api/Empresa/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutVWEmpresa(string id, VWEmpresa vWEmpresa)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != vWEmpresa.Raz_Social)
            {
                return BadRequest();
            }

            db.Entry(vWEmpresa).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VWEmpresaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Empresa
        [ResponseType(typeof(VWEmpresa))]
        public async Task<IHttpActionResult> PostVWEmpresa(VWEmpresa vWEmpresa)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.VWEmpresas.Add(vWEmpresa);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (VWEmpresaExists(vWEmpresa.Raz_Social))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = vWEmpresa.Raz_Social }, vWEmpresa);
        }

        // DELETE: api/Empresa/5
        [ResponseType(typeof(VWEmpresa))]
        public async Task<IHttpActionResult> DeleteVWEmpresa(string id)
        {
            VWEmpresa vWEmpresa = await db.VWEmpresas.FindAsync(id);
            if (vWEmpresa == null)
            {
                return NotFound();
            }

            db.VWEmpresas.Remove(vWEmpresa);
            await db.SaveChangesAsync();

            return Ok(vWEmpresa);
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