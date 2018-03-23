using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BNE.Logger.Interface;
using BNE.PessoaFisica.Domain;
using WebApi.OutputCache.V2;

namespace BNE.PessoaFisica.WebAPI.Controllers
{
    public class RankingEmailController : ApiController
    {
        private readonly ILogger _logger;
        private readonly RankingEmail _rankingEmail;

        public RankingEmailController(RankingEmail rankingEmail, ILogger logger)
        {
            _rankingEmail = rankingEmail;
            _logger = logger;
        }

        //Deflate faz a compressão do retorno. Reduz até 80%.
        //Faz o cache do método. TimeSpan em segundos.
        [CacheOutput(ClientTimeSpan = 100, ServerTimeSpan = 3600)]
        public IEnumerable<string> GetSugestaoEmail(string query, int limit)
        {
            try
            {
                return _rankingEmail.ListarTodos(query, limit);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Pessoa Fisica API - GetSugestaoEmail");
            }
            return null;
        }

        // GET api/<controller>
        public IEnumerable<string> Get(string nome)
        {
            return new[] {"@gmail.com", "@bne.com.br"};
        }

        [HttpPost]
        public HttpResponseMessage Post(string nome)
        {
            return Request.CreateResponse(HttpStatusCode.OK, new[] {"@gmail.com", "@bne.com.br"});
        }


        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        public HttpResponseMessage Options()
        {
            return new HttpResponseMessage {StatusCode = HttpStatusCode.OK};
        }
    }
}