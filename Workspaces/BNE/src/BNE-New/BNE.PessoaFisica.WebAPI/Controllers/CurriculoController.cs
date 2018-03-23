using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using BNE.PessoaFisica.ApplicationService.Curriculo.Command;
using BNE.PessoaFisica.ApplicationService.Curriculo.Interface;
using BNE.PessoaFisica.ApplicationService.Curriculo.Model;
using log4net;
using Newtonsoft.Json;
using SharedKernel.API.Controllers;
using SharedKernel.DomainEvents.Assertation;
using SharedKernel.DomainEvents.Core;
using Swashbuckle.Swagger.Annotations;

namespace BNE.PessoaFisica.WebAPI.Controllers
{
    /// <summary>
    ///     Endpoint para Curriculo
    /// </summary>
    [RoutePrefix("v1/curriculo")]
    public class CurriculoController : BaseController
    {
        private readonly ICurriculoApplicationService _curriculoApplicationService;
        private readonly ILog _logger;

        /// <summary>
        ///     Construtor do endpoint para Curriculo
        /// </summary>
        /// <param name="curriculoApplicationService"></param>
        /// <param name="logger"></param>
        public CurriculoController(ICurriculoApplicationService curriculoApplicationService, ILog logger, EventPoolHandler<AssertError> eventPool) : base(eventPool)
        {
            _curriculoApplicationService = curriculoApplicationService;
            _logger = logger;
        }

        /// <summary>
        ///     Recupera informações do currículo do bne e possível candidatura na vaga
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(InformacaoCurriculoResponse))]
        [SwaggerResponse(HttpStatusCode.NotFound, Description = "Retornado quando um código não foi encontrado ou já foi utilizado")]
        [SwaggerResponse(HttpStatusCode.BadRequest, Description = "Retornado quando houve um erro na requisição")]
        [Route("GetInformacoesCurriculo")]
        public async Task<HttpResponseMessage> GetInformacoesCurriculo([FromUri(Name = "")]RecuperarInformacaoCurriculoCommand command)
        {
            try
            {
                var informacoes = _curriculoApplicationService.CarregarInformacoesCurriculo(command);

                if (informacoes == null)
                    return await CreateErrorResponse(HttpStatusCode.NotFound, "Curriculo não encontrado.");

                return await CreateResponse(informacoes);
            }
            catch (Exception ex)
            {
                _logger.Error(JsonConvert.SerializeObject(command), ex);
                return await CreateErrorResponse(HttpStatusCode.BadRequest, "Erro.");
            }
        }

        /// <summary>
        ///     Recuperar as informações relacionadas ao currículo
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(CandidaturaDegustacao))]
        [SwaggerResponse(HttpStatusCode.NotFound, Description = "Retornado quando a quantidade de candidatura não foi encontrado")]
        [SwaggerResponse(HttpStatusCode.BadRequest, Description = "Retornado quando houve um erro na requisição")]
        [Route("GetCadidaturasDegustacao")]
        public async Task<HttpResponseMessage> GetCadidaturasDegustacao([FromUri(Name = "")]RecuperarCandidaturaDegustacaoCommand command)
        {
            try
            {
                var informacoes = _curriculoApplicationService.CarregarCadidaturasDegustacao(command);

                if (informacoes == null)
                    return await CreateErrorResponse(HttpStatusCode.NotFound, "Curriculo não encontrado.");

                return await CreateResponse(informacoes);
            }
            catch (Exception ex)
            {
                _logger.Error(JsonConvert.SerializeObject(command), ex);
                return await CreateErrorResponse(HttpStatusCode.BadRequest, "Erro.");
            }
        }

        /// <summary>
        ///     Candidatura de um curriculo
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(CandidaturaResponse))]
        [SwaggerResponse(HttpStatusCode.BadRequest, Description = "Retornado quando houve um erro na requisição")]
        [Route("Candidatar")]
        public async Task<HttpResponseMessage> Candidatar(SalvarCurriculoCommand command)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var resultado = _curriculoApplicationService.CandidatarCurriculo(command);

                    if (resultado != null)
                    {
                        return await CreateResponse(resultado);
                    }
                }
                else
                {
                    var errors = new Dictionary<string, IEnumerable<string>>();
                    foreach (var keyValue in ModelState)
                    {
                        errors[keyValue.Key] = keyValue.Value.Errors.Select(e => string.IsNullOrWhiteSpace(e.ErrorMessage) ? e.Exception.Message : e.ErrorMessage);
                    }

                    //TODO Precisar salvar isso?
                    _logger.Error("PF Candidatar - modelo inválido", new Exception(errors.ToString()));
                }
                return await CreateErrorResponse(HttpStatusCode.BadRequest, "Erro.");
            }
            catch (Exception ex)
            {
                _logger.Error(JsonConvert.SerializeObject(command), ex);
                return await CreateErrorResponse(HttpStatusCode.BadRequest, "Erro.");
            }
        }

        /// <summary>
        ///     Cadastrar Experiencia Profissional do Candidato
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ExperienciaProfissionalResponse))]
        [SwaggerResponse(HttpStatusCode.BadRequest, Description = "Retornado quando houve um erro na requisição")]
        [Route("CadastrarExperienciaProfissional")]
        public async Task<HttpResponseMessage> CadastrarExperienciaProfissional(SalvarExperienciaProfissionalCommand command)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var resultado = _curriculoApplicationService.SalvarExperienciaProfissional(command);

                    if (resultado != null)
                    {
                        return await CreateResponse(resultado);
                    }
                }

                return await CreateErrorResponse(HttpStatusCode.BadRequest, "Erro.");
            }
            catch (Exception ex)
            {
                _logger.Error(JsonConvert.SerializeObject(command), ex);
                return await CreateErrorResponse(HttpStatusCode.BadRequest, "Erro.");
            }
        }

        /// <summary>
        ///     Cadastrar Formacao Profissional do Candidato
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(FormacaoResponse))]
        [SwaggerResponse(HttpStatusCode.BadRequest, Description = "Retornado quando houve um erro na requisição")]
        [Route("CadastrarFormacao")]
        public async Task<HttpResponseMessage> CadastrarFormacao(SalvarFormacaoCommand command)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var resultado = _curriculoApplicationService.SalvarFormacao(command);

                    if (resultado != null)
                    {
                        return await CreateResponse(resultado);
                    }
                }

                return await CreateErrorResponse(HttpStatusCode.BadRequest, "Erro.");
            }
            catch (Exception ex)
            {
                _logger.Error(JsonConvert.SerializeObject(command), ex);
                return await CreateErrorResponse(HttpStatusCode.BadRequest, "Erro.");
            }
        }

        /// <summary>
        ///     Cadastra um mini-currículo para a pessoa física
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(CadastroCurriculoResponse))]
        [SwaggerResponse(HttpStatusCode.BadRequest, Description = "Retornado quando houve um erro na requisição")]
        [Route("Cadastrar")]
        public async Task<HttpResponseMessage> Cadastrar(SalvarCurriculoCommand command)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //TODO: Workaround para travar um hacker fazendo spam na api
                    if (!string.IsNullOrWhiteSpace(command.Email) && command.Email == "sample@email.tst")
                        return await CreateErrorResponse(HttpStatusCode.BadRequest, "Erro." + "sample@email.tst");

                    var retorno = _curriculoApplicationService.Cadastrar(command);

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