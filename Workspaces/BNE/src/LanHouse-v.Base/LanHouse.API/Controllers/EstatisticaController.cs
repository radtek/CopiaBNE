using LanHouse.Entities.BNE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LanHouse.API.Controllers
{
    public class EstatisticaController : ApiController
    {
        // GET: api/Estatistica/5
        public HttpResponseMessage Get()
        {
            try
            {
                BNE_Estatistica objEstatistica;
                var tem = Business.Estatistica.CarregarEstatistica(out objEstatistica);

                return Request.CreateResponse(HttpStatusCode.OK, objEstatistica);
            }
            catch (Exception ex)
            {
                string msgErro;
                Business.EL.GerenciadorException.GravarExcecao(ex, out msgErro, "Lan House API - Estatísticas BNE");
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        // POST: api/Estatistica
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Estatistica/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Estatistica/5
        public void Delete(int id)
        {
        }
    }
}
