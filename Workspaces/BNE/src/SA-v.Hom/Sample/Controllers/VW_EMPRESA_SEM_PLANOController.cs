using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using AdminLTE_Application;

namespace AdminLTE_Application.Controllers
{
    public class VW_EMPRESA_SEM_PLANOController : ApiController
    {
        private Model db = new Model();

        // GET: api/VW_EMPRESA_SEM_PLANO
        public IQueryable<VW_EMPRESA_SEM_PLANO> GetVW_EMPRESA_SEM_PLANO()
        {
            return db.VW_EMPRESA_SEM_PLANO;
        }

        // GET: api/VW_EMPRESA_SEM_PLANO/5
        [ResponseType(typeof(VW_EMPRESA_SEM_PLANO))]
        public IHttpActionResult GetVW_EMPRESA_SEM_PLANO(string id)
        {
            VW_EMPRESA_SEM_PLANO vW_EMPRESA_SEM_PLANO = db.VW_EMPRESA_SEM_PLANO.Find(id);
            if (vW_EMPRESA_SEM_PLANO == null)
            {
                return NotFound();
            }

            return Ok(vW_EMPRESA_SEM_PLANO);
        }

        // PUT: api/VW_EMPRESA_SEM_PLANO/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutVW_EMPRESA_SEM_PLANO(decimal id, VW_EMPRESA_SEM_PLANO vW_EMPRESA_SEM_PLANO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != vW_EMPRESA_SEM_PLANO.Num_CPF)
            {
                return BadRequest();
            }

            db.Entry(vW_EMPRESA_SEM_PLANO).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VW_EMPRESA_SEM_PLANOExists(id))
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

        // POST: api/VW_EMPRESA_SEM_PLANO
        [ResponseType(typeof(VW_EMPRESA_SEM_PLANO))]
        public IHttpActionResult PostVW_EMPRESA_SEM_PLANO(VW_EMPRESA_SEM_PLANO vW_EMPRESA_SEM_PLANO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.VW_EMPRESA_SEM_PLANO.Add(vW_EMPRESA_SEM_PLANO);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (VW_EMPRESA_SEM_PLANOExists(vW_EMPRESA_SEM_PLANO.Num_CPF))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = vW_EMPRESA_SEM_PLANO.Num_CPF }, vW_EMPRESA_SEM_PLANO);
        }

        // DELETE: api/VW_EMPRESA_SEM_PLANO/5
        [ResponseType(typeof(VW_EMPRESA_SEM_PLANO))]
        public IHttpActionResult DeleteVW_EMPRESA_SEM_PLANO(decimal id)
        {
            VW_EMPRESA_SEM_PLANO vW_EMPRESA_SEM_PLANO = db.VW_EMPRESA_SEM_PLANO.Find(id);
            if (vW_EMPRESA_SEM_PLANO == null)
            {
                return NotFound();
            }

            db.VW_EMPRESA_SEM_PLANO.Remove(vW_EMPRESA_SEM_PLANO);
            db.SaveChanges();

            return Ok(vW_EMPRESA_SEM_PLANO);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool VW_EMPRESA_SEM_PLANOExists(decimal id)
        {
            return db.VW_EMPRESA_SEM_PLANO.Count(e => e.Num_CPF == id) > 0;
        }
    }
}