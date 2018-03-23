using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using BNE.PessoaFisica.ApplicationService.Curso.Command;
using BNE.PessoaFisica.ApplicationService.Curso.Interface;
using BNE.PessoaFisica.ApplicationService.Curso.Model;
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
    ///     Endpoint para Curso
    /// </summary>
    [RoutePrefix("v1/curso")]
    public class CursoController : BaseController
    {
        private readonly ICursoApplicationService _cursoApplicationService;
        private readonly ILog _logger;

        /// <summary>
        ///     Construtor do endpoint para Curso
        /// </summary>
        /// <param name="cursoApplicationService"></param>
        /// <param name="logger"></param>
        public CursoController(ICursoApplicationService cursoApplicationService, ILog logger, EventPoolHandler<AssertError> eventPool) : base(eventPool)
        {
            _cursoApplicationService = cursoApplicationService;
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
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(IEnumerable<CursoResponse>))]
        [SwaggerResponse(HttpStatusCode.BadRequest, Description = "Retornado quando houve um erro na requisição")]
        [Route("GetCursos")]
        public async Task<HttpResponseMessage> GetCursos([FromUri(Name = "")] GetCursoCommand command)
        {
            try
            {
                var sugestao = _cursoApplicationService.GetList(command);

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