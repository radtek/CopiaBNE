using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using BNE.PessoaFisica.ApplicationService.CodigoConfirmacaoEmail.Command;
using BNE.PessoaFisica.ApplicationService.CodigoConfirmacaoEmail.Interface;
using log4net;
using Newtonsoft.Json;
using SharedKernel.API.Controllers;
using SharedKernel.DomainEvents.Assertation;
using SharedKernel.DomainEvents.Core;
using Swashbuckle.Swagger.Annotations;

namespace BNE.PessoaFisica.WebAPI.Controllers
{
    /// <summary>
    /// Endpoint para Código de Confirmação
    /// </summary>
    [RoutePrefix("v1/codigoconfirmacao")]
    public class CodigoConfirmacaoEmailController : BaseController
    {
        private readonly ICodigoConfirmacaoEmailApplicationService _codigoConfirmacaoEmailApplicationService;
        private readonly ILog _logger;

        /// <summary>
        /// Construtor do endpoint para Código de Confirmação
        /// </summary>
        public CodigoConfirmacaoEmailController(ICodigoConfirmacaoEmailApplicationService codigoConfirmacaoEmailApplicationService, ILog logger, EventPoolHandler<AssertError> eventPool) : base(eventPool)
        {
            _codigoConfirmacaoEmailApplicationService = codigoConfirmacaoEmailApplicationService;
            _logger = logger;
        }

        /// <summary>
        /// Recuperar um código de confirmação
        /// </summary>
        /// <param name="command">Command utilizado para recuperar um código de confirmação</param>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(string))]
        [SwaggerResponse(HttpStatusCode.NotFound, Description = "Retornado quando um código não foi encontrado ou já foi utilizado")]
        [SwaggerResponse(HttpStatusCode.BadRequest, Description = "Retornado quando houve um erro na requisição")]
        [Route("Get")]
        public async Task<HttpResponseMessage> Get([FromUri(Name = "")]GetByCodigoCommand command)
        {
            try
            {
                var codigo = _codigoConfirmacaoEmailApplicationService.GetByCodigo(command);

                if (string.IsNullOrWhiteSpace(codigo))
                    return await CreateErrorResponse(HttpStatusCode.NotFound, "Não encontrado.");

                return await CreateResponse(codigo);
            }
            catch (Exception ex)
            {
                _logger.Error(JsonConvert.SerializeObject(command), ex);
                return await CreateErrorResponse(HttpStatusCode.BadRequest, "Erro.");
            }
        }
    }
}