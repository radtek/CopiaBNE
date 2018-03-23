using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using BNE.PessoaFisica.ApplicationService.RamoAtividade.Command;
using BNE.PessoaFisica.ApplicationService.RamoAtividade.Interface;
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
    ///     Endpoint para Ramo de Atividade
    /// </summary>
    [RoutePrefix("v1/ramoatividade")]
    public class RamoAtividadeController : BaseController
    {
        private readonly IRamoAtividadeApplicationService _applicationService;
        private readonly ILog _logger;

        /// <summary>
        ///     Construtor para endpoint para Ramo de Atividade
        /// </summary>
        /// <param name="applicationService"></param>
        /// <param name="logger"></param>
        public RamoAtividadeController(IRamoAtividadeApplicationService applicationService, ILog logger, EventPoolHandler<AssertError> eventPool) : base(eventPool)
        {
            _applicationService = applicationService;
            _logger = logger;
        }

        //Deflate faz a compressão do retorno. Reduz até 80%.
        //Faz o cache do método. TimeSpan em segundos.
        /// <summary>
        ///     Recupera uma lista com Ramo de atividade
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [CacheOutput(ClientTimeSpan = 86400, ServerTimeSpan = 86400)]
        [HttpGet]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(IEnumerable<string>))]
        [SwaggerResponse(HttpStatusCode.BadRequest, Description = "Retornado quando houve um erro na requisição")]
        [Route("Get")]
        public async Task<HttpResponseMessage> Get([FromUri(Name = "")] GetRamoAtividadeCommand command)
        {
            try
            {
                var retunedObject = _applicationService.GetList(command);

                return await CreateResponse(retunedObject);
            }
            catch (Exception ex)
            {
                _logger.Error(JsonConvert.SerializeObject(command), ex);
                return await CreateErrorResponse(HttpStatusCode.BadRequest, "Erro.");
            }
        }
    }
}