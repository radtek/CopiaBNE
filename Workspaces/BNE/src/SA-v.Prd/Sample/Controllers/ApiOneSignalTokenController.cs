using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Web.Http;
using Sample.Models;

namespace AdminLTE_Application.Controllers
{
    public class ApiOneSignalTokenController : ApiController
    {
        private readonly Model db = new Model();

        [HttpPost]
        //[Route("api/OneSignalToken")]
        public async Task<IHttpActionResult> Post(OneSignalToken command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            //TODO: Seguindo o "padrão" do projeto
            using (var context = new Model())
            {
                var email = new SqlParameter("@email", command.User.Replace("dott", ".").Replace("arroba", "@"));
                var token = new SqlParameter("@token", command.Token ?? (object)DBNull.Value);

                await context.Database.ExecuteSqlCommandAsync("update dbo.CRM_Vendedor set Des_OneSignalToken = @token WHERE Eml_Vendedor = @email", email, token);
            }

            return Ok();
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