using LanHouse.Business.EL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LanHouse.API.Controllers
{
    [Authorize]
    public class EscolaridadeGrauController : LanHouse.API.Code.BaseController
    {
        // GET: api/EscolaridadeGrau
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/EscolaridadeGrau/5
        public HttpResponseMessage Get(int id)
        {
            try
            {
                string grauEscolaridade = Business.Escolaridade.CarregarGraudaEscolaridade(id);
                return Request.CreateResponse(HttpStatusCode.OK, grauEscolaridade);
            }
            catch (RecordNotFoundException ex)
            {
                string msgErro;
                Business.EL.GerenciadorException.GravarExcecao(ex, out msgErro, "Lan House API - RNF - Carregar grau da escolaridade");
                return Request.CreateResponse(HttpStatusCode.NotFound, ex.Message);
            }
            catch (Exception ex)
            {
                string msgErro;
                Business.EL.GerenciadorException.GravarExcecao(ex, out msgErro, "Lan House API - Carregar grau da escolaridade");
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        // POST: api/EscolaridadeGrau
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/EscolaridadeGrau/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/EscolaridadeGrau/5
        public void Delete(int id)
        {
        }
    }
}
