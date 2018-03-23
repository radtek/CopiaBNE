using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using BNE.PessoaFisica.ApplicationService.PessoFisica.Command;
using BNE.PessoaFisica.ApplicationService.PessoFisica.Interface;
using BNE.PessoaFisica.ApplicationService.PessoFisica.Model;
using log4net;
using Newtonsoft.Json;
using SharedKernel.API.Controllers;
using SharedKernel.DomainEvents.Assertation;
using SharedKernel.DomainEvents.Core;
using Swashbuckle.Swagger.Annotations;

namespace BNE.PessoaFisica.WebAPI.Controllers
{
    /// <summary>
    ///     Endpoint para Pessoa Fisica
    /// </summary>
    [RoutePrefix("v1/pessoafisica")]
    public class PessoaFisicaController : BaseController
    {
        private readonly ILog _logger;
        private readonly IPessoaFisicaApplicationService _pessoaFisicaApplicationService;

        /// <summary>
        ///     Contrutor de Endpoint para Pessoa Fisica
        /// </summary>
        /// <param name="pessoaFisicaApplicationService"></param>
        /// <param name="logger"></param>
        public PessoaFisicaController(IPessoaFisicaApplicationService pessoaFisicaApplicationService, ILog logger, EventPoolHandler<AssertError> eventPool) : base(eventPool)
        {
            _pessoaFisicaApplicationService = pessoaFisicaApplicationService;
            _logger = logger;
        }

        /// <summary>
        ///     Indicar amigos para vaga
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(IndicarAmigosResponse))]
        [SwaggerResponse(HttpStatusCode.BadRequest, Description = "Retornado quando houve um erro na requisição")]
        [Route("IndicarAmigos")]
        public async Task<HttpResponseMessage> IndicarAmigos(IndicarAmigosCommand command)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var retorno = _pessoaFisicaApplicationService.IndicarAmigos(command);

                    if (retorno != null)
                    {
                        return await CreateResponse(retorno);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(JsonConvert.SerializeObject(command), ex);
            }
            return await CreateErrorResponse(HttpStatusCode.BadRequest, "Erro.");
        }
    }
}