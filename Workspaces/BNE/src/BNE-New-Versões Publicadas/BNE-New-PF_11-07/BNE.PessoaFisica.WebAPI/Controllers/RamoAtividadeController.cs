using System;
using System.Collections.Generic;
using System.Web.Http;
using BNE.Logger.Interface;
using BNE.Global.Domain;
using WebApi.OutputCache.V2;

namespace BNE.PessoaFisica.WebAPI.Controllers
{
    public class RamoAtividadeController : ApiController
    {
        private readonly ILogger _logger;
        private readonly RamoAtividade _ramoAtividadeDomain;

        public RamoAtividadeController(RamoAtividade ramoAtividade, ILogger logger)
        {
            _ramoAtividadeDomain = ramoAtividade;
            _logger = logger;
        }

        //Deflate faz a compressão do retorno. Reduz até 80%.
        //Faz o cache do método. TimeSpan em segundos.
        [CacheOutput(ClientTimeSpan = 100, ServerTimeSpan = 100)]
        public IEnumerable<string> Get(string query, int limit)
        {
            try
            {
                return _ramoAtividadeDomain.ListaRamoAtividades(query);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Pessoa Fisica API - RamoAtividade.Get");
            }
            return null;
        }
    }
}