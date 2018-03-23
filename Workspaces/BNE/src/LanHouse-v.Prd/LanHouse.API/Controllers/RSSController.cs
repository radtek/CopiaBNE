using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LanHouse.API.Controllers
{
    public class RSSController : ApiController
    {
        // GET: api/RSS
        public HttpResponseMessage Get()
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, Business.RssRead.LerRssNoticiasAzulzinho());
            }
            catch (Exception ex)
            {
                string msgErro;
                Business.EL.GerenciadorException.GravarExcecao(ex, out msgErro, "Lan House API - Ler Rss do noticias.azulzinho.com.br");
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        // GET: api/RSS/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/RSS
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/RSS/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/RSS/5
        public void Delete(int id)
        {
        }
    }
}
