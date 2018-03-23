using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using BNE.PessoaFisica.ApplicationService.DadosEmpresa.Command;
using BNE.PessoaFisica.ApplicationService.DadosEmpresa.Interface;
using BNE.PessoaFisica.ApplicationService.DadosEmpresa.Model;
using log4net;
using Newtonsoft.Json;
using SharedKernel.API.Controllers;
using SharedKernel.DomainEvents.Assertation;
using SharedKernel.DomainEvents.Core;
using Swashbuckle.Swagger.Annotations;

namespace BNE.PessoaFisica.WebAPI.Controllers
{
    /// <summary>
    ///     Endpoint para dados empresa
    /// </summary>
    [RoutePrefix("v1/DadosEmpresa")]
    public class DadosEmpresaController : BaseController
    {
        private readonly ILog _logger;

        private readonly IDadosEmpresaApplicationService _dadosEmpresaApplicationService;

        /// <summary>
        ///     Contrutor do endpoint para dados empresa
        /// </summary>
        /// <param name="DadosEmpresaApplicationService"></param>
        /// <param name="logger"></param>
        public DadosEmpresaController(IDadosEmpresaApplicationService dadosEmpresaApplicationService, ILog logger, EventPoolHandler<AssertError> eventPool) : base(eventPool)
        {
            _dadosEmpresaApplicationService = dadosEmpresaApplicationService;
            _logger = logger;
        }

        [HttpGet]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(DadosEmpresaResponse))]
        [SwaggerResponse(HttpStatusCode.NotFound, Description = "Retornado quando não tem um empresa para o IdVaga.")]
        [SwaggerResponse(HttpStatusCode.BadRequest, Description = "Retornado quando houve um erro na requisição")]
        [Route("getDados")]
        //[CacheOutput(ClientTimeSpan = 300, ServerTimeSpan = 300)]
        public async Task<HttpResponseMessage> getDados([FromUri(Name = "")] DadosEmpresaCommand command)
        {
            try
            {
                DadosEmpresaResponse dadosResponse = null;

                await Task.WhenAny(Task.Run(() => dadosResponse = _dadosEmpresaApplicationService.getDados(command)), Task.Delay(TimeSpan.FromSeconds(10)));

                if (dadosResponse == null)
                    return await CreateErrorResponse(HttpStatusCode.NotFound, "Não encontrado.");

                return await CreateResponse(dadosResponse);
            }
            catch (Exception ex)
            {
                _logger.Error(JsonConvert.SerializeObject(command), ex);
                return await CreateErrorResponse(HttpStatusCode.BadRequest, "Erro.");
            }
        }
    }
}