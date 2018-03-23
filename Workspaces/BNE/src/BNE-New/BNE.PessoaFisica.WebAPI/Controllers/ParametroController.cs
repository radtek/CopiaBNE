using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using BNE.PessoaFisica.ApplicationService.Parametro.Interface;
using log4net;
using SharedKernel.API.Controllers;
using SharedKernel.DomainEvents.Assertation;
using SharedKernel.DomainEvents.Core;
using Swashbuckle.Swagger.Annotations;
using WebApi.OutputCache.V2;

namespace BNE.PessoaFisica.WebAPI.Controllers
{
    /// <summary>
    ///     Endpoint para Parametros
    /// </summary>
    [RoutePrefix("v1/parametro")]
    public class ParametroController : BaseController
    {
        private readonly ILog _logger;
        private readonly IParametroApplicationService _parametroApplicationService;

        /// <summary>
        ///     Construtor do endpoint para Parametros
        /// </summary>
        /// <param name="parametroApplicationService"></param>
        /// <param name="logger"></param>
        public ParametroController(IParametroApplicationService parametroApplicationService, ILog logger, EventPoolHandler<AssertError> eventPool) : base(eventPool)
        {
            _parametroApplicationService = parametroApplicationService;
            _logger = logger;
        }

        /// <summary>
        ///     Recupera o valor do salário mínimo nacional
        /// </summary>
        /// <returns></returns>
        [CacheOutput(ClientTimeSpan = 86400, ServerTimeSpan = 86400)]
        [HttpGet]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(decimal))]
        [SwaggerResponse(HttpStatusCode.BadRequest, Description = "Retornado quando houve um erro na requisição")]
        [Route("GetMinimoNacional")]
        public async Task<HttpResponseMessage> GetMinimoNacional()
        {
            try
            {
                var returnedObject = _parametroApplicationService.RecuperarValorMinimoNacional();

                return await CreateResponse(returnedObject);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return await CreateErrorResponse(HttpStatusCode.BadRequest, "Erro.");
            }
        }
    }
}