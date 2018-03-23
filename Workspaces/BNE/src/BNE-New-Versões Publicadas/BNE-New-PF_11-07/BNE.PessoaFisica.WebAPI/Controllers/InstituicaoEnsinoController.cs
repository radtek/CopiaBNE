using System;
using System.Collections.Generic;
using System.Web.Http;
using BNE.Logger.Interface;
using BNE.PessoaFisica.Domain;
using WebApi.OutputCache.V2;

namespace BNE.PessoaFisica.WebAPI.Controllers
{
    public class InstituicaoEnsinoController : ApiController
    {
        private readonly InstituicaoEnsino _instituicaoEnsinoDomain;
        private readonly ILogger _logger;

        public InstituicaoEnsinoController(InstituicaoEnsino instituicaoEnsinoDomain, ILogger logger)
        {
            _instituicaoEnsinoDomain = instituicaoEnsinoDomain;
            _logger = logger;
        }

        //Deflate faz a compressão do retorno. Reduz até 80%.
        //Faz o cache do método. TimeSpan em segundos.
        [CacheOutput(ClientTimeSpan = 100, ServerTimeSpan = 100)]
        public IEnumerable<string> GetSugestaoEmail(string query, int limit)
        {
            try
            {
                return _instituicaoEnsinoDomain.ListaSugestaoInstituicaoEnsino(query, limit);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Pessoa Fisica API - InstituicaoEnsino.GetSugestaoEmail");
            }
            return null;
        }
    }
}