using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using BNE.PessoaFisica.ApplicationService.InstituicaoEnsino.Command;
using BNE.PessoaFisica.ApplicationService.InstituicaoEnsino.Interface;
using BNE.PessoaFisica.ApplicationService.InstituicaoEnsino.Model;
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
    ///     Endpoint para Instituicao de Ensino
    /// </summary>
    [RoutePrefix("v1/instituicao")]
    public class InstituicaoEnsinoController : BaseController
    {
        private readonly IInstituicaoEnsinoApplicationService _instituicaoEnsinoApplicationService;
        private readonly ILog _logger;

        /// <summary>
        ///     Construtor do endpoint para Instituicao de Ensino
        /// </summary>
        /// <param name="instituicaoEnsinoApplicationService"></param>
        /// <param name="logger"></param>
        public InstituicaoEnsinoController(IInstituicaoEnsinoApplicationService instituicaoEnsinoApplicationService, ILog logger, EventPoolHandler<AssertError> eventPool) : base(eventPool)
        {
            _instituicaoEnsinoApplicationService = instituicaoEnsinoApplicationService;
            _logger = logger;
        }

        //Deflate faz a compressão do retorno. Reduz até 80%.
        //Faz o cache do método. TimeSpan em segundos.
        /// <summary>
        ///     Recupera as sugestões de email
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [CacheOutput(ClientTimeSpan = 86400, ServerTimeSpan = 86400)]
        [HttpGet]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(IEnumerable<InstituicaoEnsinoResponse>))]
        [SwaggerResponse(HttpStatusCode.BadRequest, Description = "Retornado quando houve um erro na requisição")]
        [Route("GetInstituicaoEnsinos")]
        public async Task<HttpResponseMessage> GetInstituicaoEnsinos([FromUri(Name = "")] GetInstituicaoEnsinoCommand command)
        {
            try
            {
                var sugestao = _instituicaoEnsinoApplicationService.GetList(command);

                return await CreateResponse(sugestao);
            }
            catch (Exception ex)
            {
                _logger.Error(JsonConvert.SerializeObject(command), ex);
                return await CreateErrorResponse(HttpStatusCode.BadRequest, "Erro.");
            }
        }
    }
}