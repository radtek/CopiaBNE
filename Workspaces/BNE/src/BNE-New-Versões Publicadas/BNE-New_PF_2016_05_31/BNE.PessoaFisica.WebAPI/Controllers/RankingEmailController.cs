using BNE.PessoaFisica.WebAPI.Attributes;
using System.Web.Http;
using WebApi.OutputCache.V2;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using WebApi.OutputCache.V2.TimeAttributes;

namespace BNE.PessoaFisica.WebAPI.Controllers
{
    public class RankingEmailController : ApiController
    {
        private readonly Domain.RankingEmail _rankingEmail;

        public RankingEmailController(Domain.RankingEmail rankingEmail)
        {
            _rankingEmail = rankingEmail;
        }

        //Deflate faz a compressão do retorno. Reduz até 80%.
        //Faz o cache do método. TimeSpan em segundos.
        [CacheOutput(ClientTimeSpan=100,ServerTimeSpan=3600)]
        public IEnumerable<string> GetSugestaoEmail(string query, int limit)
        {
           var obj = _rankingEmail.ListarTodos(query,limit);
           return obj;
        }

        // GET api/<controller>
        public IEnumerable<string> Get(string nome)
        {
            return new string[] { "@gmail.com", "@bne.com.br" };
        }
        [HttpPost]
        public HttpResponseMessage Post(string nome)
        {
            return Request.CreateResponse(HttpStatusCode.OK, new string[] { "@gmail.com", "@bne.com.br" });
        }


        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        public HttpResponseMessage Options()
        {
            return new HttpResponseMessage { StatusCode = HttpStatusCode.OK };
        }

    }
}
