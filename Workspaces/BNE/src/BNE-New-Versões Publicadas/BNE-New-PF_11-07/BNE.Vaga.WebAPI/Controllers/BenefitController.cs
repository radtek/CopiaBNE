using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;

namespace BNE.Vaga.WebAPI.Controllers
{
    /// <summary>
    /// Implementa os endpoists para os benefícios
    /// </summary>
    public class BenefitController : ApiController
    {
        private readonly Domain.Beneficio _beneficio;

        public BenefitController(Domain.Beneficio beneficio)
        {
            _beneficio = beneficio;
        }

        // GET: api/Benefit
        public HttpResponseMessage Get(String suffix = null, Guid? pessoaJuridica = null)
        {
            return Request.CreateResponse(HttpStatusCode.OK,new string[] { "value1", "value2" });
        }

        // GET: api/Benefit/5
        public HttpResponseMessage Get(int id)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, _beneficio.GetById(id));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        // POST: api/Benefit
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Benefit/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Benefit/5
        public void Delete(int id)
        {
        }
    }
}
