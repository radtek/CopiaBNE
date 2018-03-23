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

namespace Sample.Controllers
{
    public class ApiIndicadorCvsLocalidadeController : ApiController
    {
        private Model db = new Model();

        // GET: api/ApiIndicadorCvsLocalidade
        public IQueryable<VWEmpresa> GetVWEmpresas()
        {
            return db.VWEmpresas;
        }

        // GET: api/ApiIndicadorCvsLocalidade/5
        [ResponseType(typeof(VWEmpresa))]
        public async Task<IHttpActionResult> GetVWEmpresa(decimal id)
        {
            decimal Idf_Cidade = Convert.ToInt64(id);

            using (var context = new Model())
            {
                var filial = new SqlParameter("@Idf_Cidade", Idf_Cidade);

                var result = context.Database
                    .SqlQuery<NumeroCvsCidade>("dbo.getTotalCVsRegiaoEmpresa @Idf_Cidade", filial)
                    .ToList();

                return this.Ok(result);
            }
        }

        // PUT: api/ApiIndicadorCvsLocalidade/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutVWEmpresa(decimal id, VWEmpresa vWEmpresa)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != vWEmpresa.Num_CNPJ)
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

        // POST: api/ApiIndicadorCvsLocalidade
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
                if (VWEmpresaExists(vWEmpresa.Num_CNPJ))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = vWEmpresa.Num_CNPJ }, vWEmpresa);
        }

        // DELETE: api/ApiIndicadorCvsLocalidade/5
        [ResponseType(typeof(VWEmpresa))]
        public async Task<IHttpActionResult> DeleteVWEmpresa(decimal id)
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

        private bool VWEmpresaExists(decimal id)
        {
            return db.VWEmpresas.Count(e => e.Num_CNPJ == id) > 0;
        }
    }
}