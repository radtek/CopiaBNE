using LanHouse.Business.EL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace LanHouse.API.Controllers
{
    [Authorize]
    public class Escolaridade3GrauController : LanHouse.API.Code.BaseController
    {
        // GET: api/Escolaridade3Grau
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Escolaridade3Grau/5
        public HttpResponseMessage Get(int id)
        {
            try
            {
                bool is3GrauCompleto = Business.Escolaridade.ChecarNiveldaEscolaridade(id);
                return Request.CreateResponse(HttpStatusCode.OK, is3GrauCompleto.ToString());
            }
            catch (RecordNotFoundException ex)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, ex.Message);
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        // POST: api/Escolaridade3Grau
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Escolaridade3Grau/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Escolaridade3Grau/5
        public void Delete(int id)
        {
        }
    }
}
