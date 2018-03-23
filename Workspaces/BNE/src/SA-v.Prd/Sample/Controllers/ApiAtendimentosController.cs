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

namespace AdminLTE_Application.Controllers
{
    public class ApiAtendimentosController : ApiController
    {
        private Model db = new Model();

        // GET: api/Atendimentos
        public IQueryable<VWAtendimentos> GetVWAtendimentos()
        {
            return db.VWAtendimentos;
        }

        // GET: api/Atendimentos/5
        [ResponseType(typeof(VWAtendimentos))]
        public async Task<IHttpActionResult> GetVWAtendimentos(decimal id)
        {
            decimal Num_CNPJ = Convert.ToDecimal(id);

            var atendimentos = (from q in db.VWAtendimentos
                          where q.CNPJ == Num_CNPJ
                          orderby q.Dta_Atendimento descending 
                          select new
                          {
                                CNPJ = q.CNPJ,
                                Tipo_Atendimento = q.Tipo_Atendimento,
                                CPF = q.CPF,
                                Idf_Tipo_Atendimento = q.Idf_Tipo_Atendimento,
                                Dta_Atendimento = q.Dta_Atendimento,
                                Flg_Administrador = q.Flg_Administrador,
                                Des_Observacao = q.Des_Observacao,
                                Nme_Vendedor = q.Nme_Vendedor
                          } ).Take(10);

            if (atendimentos == null)
            {
                return NotFound();
            }

            return Ok(atendimentos);
        }
       

        // PUT: api/Atendimentos/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutVWAtendimentos(decimal id, VWAtendimentos VWAtendimentos)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != VWAtendimentos.CNPJ)
            {
                return BadRequest();
            }

            db.Entry(VWAtendimentos).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VWAtendimentosExists(id))
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

        // POST: api/Atendimentos
        [ResponseType(typeof(VWAtendimentos))]
        public async Task<IHttpActionResult> PostVWAtendimentos(VWAtendimentos VWAtendimentos)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.VWAtendimentos.Add(VWAtendimentos);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (VWAtendimentosExists(VWAtendimentos.CNPJ))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = VWAtendimentos.CNPJ }, VWAtendimentos);
        }

        // DELETE: api/Atendimentos/5
        [ResponseType(typeof(VWAtendimentos))]
        public async Task<IHttpActionResult> DeleteVWAtendimentos(decimal id)
        {
            VWAtendimentos VWAtendimentos = await db.VWAtendimentos.FindAsync(id);
            if (VWAtendimentos == null)
            {
                return NotFound();
            }

            db.VWAtendimentos.Remove(VWAtendimentos);
            await db.SaveChangesAsync();

            return Ok(VWAtendimentos);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool VWAtendimentosExists(decimal id)
        {
            return db.VWAtendimentos.Count(e => e.CNPJ == id) > 0;
        }
    }
}