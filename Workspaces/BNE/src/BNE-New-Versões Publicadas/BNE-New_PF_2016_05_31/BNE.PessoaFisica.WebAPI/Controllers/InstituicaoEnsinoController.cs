using BNE.PessoaFisica.WebAPI.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.OutputCache.V2;

namespace BNE.PessoaFisica.WebAPI.Controllers
{
    public class InstituicaoEnsinoController : ApiController
    {
        private readonly Domain.InstituicaoEnsino _instituicaoEnsinoDomain;

        public InstituicaoEnsinoController(Domain.InstituicaoEnsino instituicaoEnsinoDomain)
        {
            _instituicaoEnsinoDomain = instituicaoEnsinoDomain;
        }

        //Deflate faz a compressão do retorno. Reduz até 80%.
        //Faz o cache do método. TimeSpan em segundos.
        [CacheOutput(ClientTimeSpan = 100, ServerTimeSpan = 100)]
        public IEnumerable<string> GetSugestaoEmail(string query, int limit)
        {
            var obj = _instituicaoEnsinoDomain.ListaSugestaoInstituicaoEnsino(query, limit);
            return obj;
        }
    }
}
