using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace APIGateway.Controllers
{
    public class SistemaController : ApiController
    {
        // GET: api/Sistema
        public HttpResponseMessage Get()
        {
            return Request.CreateResponse(Domain.SistemaCliente.List());
        }

        // GET: api/Sistema/5
        public HttpResponseMessage Get(Guid id)
        {
            return Request.CreateResponse(Domain.SistemaCliente.Get(id));
        }

        // GET: api/Sistema/5
        public HttpResponseMessage Get(String apiUrlSuffix, bool reverse = false)
        {
            return Request.CreateResponse(Domain.SistemaCliente.List(apiUrlSuffix, reverse));
        }

        // POST: api/Sistema
        public HttpResponseMessage Post([FromBody]Model.SistemaCliente sistema)
        {
            if (sistema != null && ModelState.IsValid)
            {
                try
                {
                    return Request.CreateResponse(HttpStatusCode.Created, Domain.SistemaCliente.Add(sistema.Nome));
                }
                catch (BNE.Core.Exceptions.PrimaryKeyException)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Já existe um sistema com esse nome");
                }
            }
            
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);

        }

        // PUT: api/Sistema/5
        public HttpResponseMessage Put(Guid id, [FromBody]string nomeSistema)
        {
            return Request.CreateResponse(HttpStatusCode.OK, Domain.SistemaCliente.Update(id, nomeSistema));
        }

        // DELETE: api/Sistema/5
        public HttpResponseMessage Delete(Guid id)
        {
            try
            {
                Domain.SistemaCliente.Delete(id);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (BNE.Core.Exceptions.ForeingKeyException)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Esse sistema tem permissões em algumas API's");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }
    }
}
