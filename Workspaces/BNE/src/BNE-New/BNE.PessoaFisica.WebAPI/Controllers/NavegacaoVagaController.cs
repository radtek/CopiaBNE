using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using BNE.PessoaFisica.ApplicationService.NavegacaoVaga.Command;
using BNE.PessoaFisica.ApplicationService.NavegacaoVaga.Interface;
using BNE.PessoaFisica.ApplicationService.NavegacaoVaga.Model;
using log4net;
using Newtonsoft.Json;
using SharedKernel.API.Controllers;
using SharedKernel.DomainEvents.Assertation;
using SharedKernel.DomainEvents.Core;
using Swashbuckle.Swagger.Annotations;

namespace BNE.PessoaFisica.WebAPI.Controllers
{
    [RoutePrefix("v1/navegacaovaga")]
    public class NavegacaoVagaController : BaseController
    {
        private readonly IJobNavigationApplicationService _applicationService;
        private readonly ILog _logger;

        public NavegacaoVagaController(EventPoolHandler<AssertError> eventPool, ILog log, IJobNavigationApplicationService applicationService, ILog logger) : base(eventPool, log)
        {
            _applicationService = applicationService;
            _logger = logger;
        }


        [HttpGet]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(JobNavigationResponse))]
        [SwaggerResponse(HttpStatusCode.BadRequest, Description = "Retornado quando houve um erro na requisição")]
        [Route("Get")]
        //[CacheOutput(ClientTimeSpan = 300, ServerTimeSpan = 300)]
        public async Task<HttpResponseMessage> Get([FromUri(Name = "")] JobNavigationCommand command)
        {
            try
            {
                JobNavigationResponse response = null;

                await Task.WhenAny(Task.Run(async () => response = await _applicationService.GetNavigation(command)), Task.Delay(TimeSpan.FromSeconds(2)));

                if (response == null)
                {
                    return await CreateErrorResponse(HttpStatusCode.NotFound, "Não encontrado.");
                }

                return await CreateResponse(response);
            }
            catch (Exception ex)
            {
                _logger.Error(JsonConvert.SerializeObject(command), ex);
                return await CreateErrorResponse(HttpStatusCode.BadRequest, "Erro.");
            }
        }
    }
}