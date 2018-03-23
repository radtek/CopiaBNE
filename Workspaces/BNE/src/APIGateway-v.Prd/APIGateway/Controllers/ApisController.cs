using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace APIGateway.Controllers
{
    public class ApisController : ApiController
    {
        // GET: api/Api
        public HttpResponseMessage Get()
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, Domain.Api.Listar());
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }

        }

        // GET: api/Api/5
        public HttpResponseMessage Get(string id)
        {
            return Request.CreateResponse(HttpStatusCode.OK, Domain.Api.CarregarPorSuffix(id.Replace('_','/')));
        }

        // POST: api/Api
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Api/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Api/5
        public void Delete(int id)
        {
        }

        // GET: api/Api/5
        [HttpPost]
        public HttpResponseMessage GrantAccess(string id, Guid chaveSistema)
        {
            Domain.Api.ConcederAcesso(id.Replace('_', '/'), chaveSistema);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // GET: api/Api/5
        [HttpPost]
        public HttpResponseMessage DenyAccess(string id, Guid chaveSistema)
        {
            Domain.Api.RetirarAcesso(id.Replace('_', '/'), chaveSistema);
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
