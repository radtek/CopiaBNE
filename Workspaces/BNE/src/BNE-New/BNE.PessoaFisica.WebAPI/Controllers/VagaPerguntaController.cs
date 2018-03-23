using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using BNE.PessoaFisica.ApplicationService.VagaPergunta.Command;
using BNE.PessoaFisica.ApplicationService.VagaPergunta.Interface;
using BNE.PessoaFisica.ApplicationService.VagaPergunta.Model;
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
    ///     Endpoint para Perguntas da Vaga
    /// </summary>
    [RoutePrefix("v1/vagapergunta")]
    public class VagaPerguntaController : BaseController
    {
        private readonly IVagaPerguntaApplicationService _applicationService;
        private readonly ILog _logger;

        /// <summary>
        ///     Contrutor do endpoint para Perguntas da Vaga
        /// </summary>
        /// <param name="applicationService"></param>
        /// <param name="logger"></param>
        public VagaPerguntaController(IVagaPerguntaApplicationService applicationService, ILog logger, EventPoolHandler<AssertError> eventPool) : base(eventPool)
        {
            _applicationService = applicationService;
            _logger = logger;
        }

        /// <summary>
        ///     Recuperar uma lista com as perguntas da vaga
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(IEnumerable<VagaPerguntaResponse>))]
        [SwaggerResponse(HttpStatusCode.NotFound, Description = "Retornado quando não tem Pergunta para Vaga.")]
        [SwaggerResponse(HttpStatusCode.BadRequest, Description = "Retornado quando houve um erro na requisição")]
        [Route("GetPerguntas")]
        [CacheOutput(ClientTimeSpan = 300, ServerTimeSpan = 300)]
        public async Task<HttpResponseMessage> GetPerguntas([FromUri(Name = "")]GetByIdVagaCommand command)
        {
            try
            {
                var result = _applicationService.CarregarPergunta(command);
                if (result == null)
                    return await CreateErrorResponse(HttpStatusCode.NotFound, "Não encontrado.");

                return await CreateResponse(result);
            }
            catch (Exception ex)
            {
                _logger.Error(JsonConvert.SerializeObject(command), ex);
                return await CreateErrorResponse(HttpStatusCode.BadRequest, "Erro");
            }
        }
    }
}