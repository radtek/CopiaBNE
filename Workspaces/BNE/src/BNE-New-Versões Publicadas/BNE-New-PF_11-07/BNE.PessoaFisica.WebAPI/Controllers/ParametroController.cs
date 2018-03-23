using System;
using System.Web.Http;
using BNE.Logger.Interface;
using BNE.PessoaFisica.Domain;
using WebApi.OutputCache.V2;

namespace BNE.PessoaFisica.WebAPI.Controllers
{
    public class ParametroController : ApiController
    {
        private readonly ILogger _logger;
        private readonly Parametro _parametroDomain;

        public ParametroController(Parametro parametroDomain, ILogger logger)
        {
            _parametroDomain = parametroDomain;
            _logger = logger;
        }

        [CacheOutput(ClientTimeSpan = 100, ServerTimeSpan = 100)]
        public string GetMinimoNacional()
        {
            try
            {
                return _parametroDomain.RecuperarValor(Model.Enumeradores.Parametro.SalarioMinimoNacional);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Pessoa Fisica API - Parametro.GetMinimoNacional");
            }
            return null;
        }
    }
}