using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using BNE.PessoaFisica.ApplicationService.PreCurriculo.Command;
using BNE.PessoaFisica.ApplicationService.PreCurriculo.Interface;
using BNE.PessoaFisica.ApplicationService.PreCurriculo.Model;
using log4net;
using Newtonsoft.Json;
using SharedKernel.API.Controllers;
using SharedKernel.DomainEvents.Assertation;
using SharedKernel.DomainEvents.Core;
using Swashbuckle.Swagger.Annotations;

namespace BNE.PessoaFisica.WebAPI.Controllers
{
    /// <summary>
    ///     Endpoint para PreCurriculo
    /// </summary>
    [RoutePrefix("v1/precurriculo")]
    public class PreCurriculoController : BaseController
    {
        private readonly ILog _logger;
        private readonly IPreCurriculoApplicationService _preCurriculoApplicationService;

        /// <summary>
        ///     Contrutor do endpoint para PreCurriculo
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="preCurriculoApplicationService"></param>
        /// <param name="eventPool"></param>
        public PreCurriculoController(IPreCurriculoApplicationService preCurriculoApplicationService, ILog logger, EventPoolHandler<AssertError> eventPool) : base(eventPool)
        {
            _logger = logger;
            _preCurriculoApplicationService = preCurriculoApplicationService;
        }

        /// <summary>
        ///     Cadastra um minicurriculo
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(PreCurriculoResponse))]
        [SwaggerResponse(HttpStatusCode.BadRequest, Description = "Retornado quando houve um erro na requisição")]
        [Route("Cadastrar")]
        public async Task<HttpResponseMessage> Cadastrar(SalvarPreCurriculoCommand command)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var retorno = _preCurriculoApplicationService.Cadastrar(command);

                    if (retorno != null)
                    {
                        return await CreateResponse(retorno);
                    }
                }
                else
                {
                    var modelErrors = string.Join(",", ModelState.Values.SelectMany(x => x.Errors).Select(x => string.IsNullOrWhiteSpace(x.ErrorMessage) ? x.Exception.Message : x.ErrorMessage));

                    return await CreateErrorResponse(HttpStatusCode.BadRequest, modelErrors);
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