using System;
using System.Collections.Generic;
using System.Web.Http;
using BNE.Logger.Interface;
using BNE.PessoaFisica.Domain;
using WebApi.OutputCache.V2;

namespace BNE.PessoaFisica.WebAPI.Controllers
{
    public class CursoController : ApiController
    {
        private readonly Curso _cursoDomain;
        private readonly ILogger _logger;

        public CursoController(Curso cursoDomain, ILogger logger)
        {
            _cursoDomain = cursoDomain;
            _logger = logger;
        }

        //Deflate faz a compressão do retorno. Reduz até 80%.
        //Faz o cache do método. TimeSpan em segundos.
        [CacheOutput(ClientTimeSpan = 100, ServerTimeSpan = 100)]
        public IEnumerable<string> GetSugestaoEmail(string query, int limit)
        {
            try
            {
                return _cursoDomain.ListaSugestaoCurso(query, limit);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Pessoa Fisica API - Curso.GetSugestaoEmail");
            }
            return null;
        }
    }
}