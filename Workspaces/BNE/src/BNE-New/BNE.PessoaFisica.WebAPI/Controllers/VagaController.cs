using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using BNE.PessoaFisica.ApplicationService.Vaga.Command;
using BNE.PessoaFisica.ApplicationService.Vaga.Interface;
using BNE.PessoaFisica.ApplicationService.Vaga.Model;
using log4net;
using Newtonsoft.Json;
using SharedKernel.API.Controllers;
using SharedKernel.DomainEvents.Assertation;
using SharedKernel.DomainEvents.Core;
using Swashbuckle.Swagger.Annotations;
using WebApi.OutputCache.V2;

namespace BNE.PessoaFisica.WebAPI.Controllers
{
    /// <summary>
    ///     Endpoint para Vaga
    /// </summary>
    [RoutePrefix("v1/vaga")]
    public class VagaController : BaseController
    {
        private readonly IVagaApplicationService _applicationService;
        private readonly ILog _logger;

        /// <summary>
        ///     Contrutor do endpoint para Vaga
        /// </summary>
        /// <param name="applicationService"></param>
        /// <param name="logger"></param>
        public VagaController(IVagaApplicationService applicationService, ILog logger, EventPoolHandler<AssertError> eventPool) : base(eventPool)
        {
            _applicationService = applicationService;
            _logger = logger;
        }

        /// <summary>
        ///     Recupera uma vaga pelo ID
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(VagaResponse))]
        [SwaggerResponse(HttpStatusCode.NotFound, Description = "Retornado quando não tem um Vaga para o ID.")]
        [SwaggerResponse(HttpStatusCode.BadRequest, Description = "Retornado quando houve um erro na requisição")]
        [Route("Get")]
        //[CacheOutput(ClientTimeSpan = 300, ServerTimeSpan = 300)]
        public async Task<HttpResponseMessage> Get([FromUri(Name = "")] GetByIdCommand command)
        {
            try
            {
                VagaResponse vagaResponse = null;

                await Task.WhenAny(Task.Run(() => vagaResponse = _applicationService.CarregarVaga(command)), Task.Delay(TimeSpan.FromSeconds(10)));

                if (vagaResponse == null)
                    return await CreateErrorResponse(HttpStatusCode.NotFound, "Não encontrado.");

                return await CreateResponse(vagaResponse);
            }
            catch (Exception ex)
            {
                _logger.Error(JsonConvert.SerializeObject(command), ex);
                return await CreateErrorResponse(HttpStatusCode.BadRequest, "Erro.");
            }
        }
    }
}