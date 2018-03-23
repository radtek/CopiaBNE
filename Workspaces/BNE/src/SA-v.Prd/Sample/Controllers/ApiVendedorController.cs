using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using AdminLTE_Application;

namespace Sample.Controllers
{
    public class ApiVendedorController : ApiController
    {
        private Model db = new Model();

        // GET: api/ApiVendedor
        public async Task<IHttpActionResult> GetCRM_Vendedor()
        {
            //return db.CRM_Vendedor;
            using (var context = new Model())
            {
             
                var result = context.Database
                    .SqlQuery<CRM_Vendedor>("dbo.getVendedores")
                    .ToList();

                return this.Ok(result);
            }
        }

        // GET: api/ApiVendedor/5
        [ResponseType(typeof(CRM_Vendedor))]
        public async Task<IHttpActionResult> GetCRM_Vendedor(decimal id)
        {
            CRM_Vendedor cRM_Vendedor = await db.CRM_Vendedor.FindAsync(id);
            if (cRM_Vendedor == null)
            {
                return NotFound();
            }

            return Ok(cRM_Vendedor);
        }

        // PUT: api/ApiVendedor/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCRM_Vendedor(decimal id, CRM_Vendedor cRM_Vendedor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != cRM_Vendedor.Num_CPF)
            {
                return BadRequest();
            }

            db.Entry(cRM_Vendedor).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CRM_VendedorExists(id))
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

        // POST: api/ApiVendedor
        [ResponseType(typeof(CRM_Vendedor))]
        public async Task<IHttpActionResult> PostCRM_Vendedor(CRM_Vendedor cRM_Vendedor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.CRM_Vendedor.Add(cRM_Vendedor);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CRM_VendedorExists(cRM_Vendedor.Num_CPF))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = cRM_Vendedor.Num_CPF }, cRM_Vendedor);
        }

        // DELETE: api/ApiVendedor/5
        [ResponseType(typeof(CRM_Vendedor))]
        public async Task<IHttpActionResult> DeleteCRM_Vendedor(decimal id)
        {
            CRM_Vendedor cRM_Vendedor = await db.CRM_Vendedor.FindAsync(id);
            if (cRM_Vendedor == null)
            {
                return NotFound();
            }

            db.CRM_Vendedor.Remove(cRM_Vendedor);
            await db.SaveChangesAsync();

            return Ok(cRM_Vendedor);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CRM_VendedorExists(decimal id)
        {
            return db.CRM_Vendedor.Count(e => e.Num_CPF == id) > 0;
        }
    }
}