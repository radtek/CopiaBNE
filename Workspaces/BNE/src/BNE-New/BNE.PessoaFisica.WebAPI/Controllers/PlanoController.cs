using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using BNE.PessoaFisica.ApplicationService.Plano.Command;
using BNE.PessoaFisica.ApplicationService.Plano.Interface;
using BNE.PessoaFisica.ApplicationService.Plano.Model;
using log4net;
using Newtonsoft.Json;
using SharedKernel.API.Controllers;
using SharedKernel.DomainEvents.Assertation;
using SharedKernel.DomainEvents.Core;
using Swashbuckle.Swagger.Annotations;

namespace BNE.PessoaFisica.WebAPI.Controllers
{
    /// <summary>
    ///     Endpoint para plano
    /// </summary>
    [RoutePrefix("v1/plano")]
    public class PlanoController : BaseController
    {
        private readonly ILog _logger;

        private readonly IPlanoApplicationService _planoApplicationService;

        /// <summary>
        ///     Contrutor do endpoint para plano
        /// </summary>
        /// <param name="planoApplicationService"></param>
        /// <param name="logger"></param>
        public PlanoController(IPlanoApplicationService planoApplicationService, ILog logger, EventPoolHandler<AssertError> eventPool) : base(eventPool)
        {
            _planoApplicationService = planoApplicationService;
            _logger = logger;
        }

        /// <summary>
        ///     Recupera um plano
        /// </summary>
        /// <param name="command">Id do plano</param>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(PlanoResponse))]
        [SwaggerResponse(HttpStatusCode.NotFound, Description = "Retornado quando um plano não foi encontrado")]
        [SwaggerResponse(HttpStatusCode.BadRequest, Description = "Retornado quando algum erro interno ocorreu")]
        [Route("GetPlano")]
        public async Task<HttpResponseMessage> GetPlano([FromUri(Name = "")]GetPlanoCommand command)
        {
            try
            {
                var result = _planoApplicationService.GetPlano(command);
                if (result == null)
                    return await CreateErrorResponse(HttpStatusCode.NotFound, "Não encontrado.");
                return await CreateResponse(result);
            }
            catch (Exception ex)
            {
                _logger.Error("Pessoa Fisica API - Pegar o plano para vaga premium " + JsonConvert.SerializeObject(command), ex);
                return await CreateErrorResponse(HttpStatusCode.BadRequest, "Erro.");
            }
        }
    }
}