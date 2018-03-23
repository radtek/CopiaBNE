using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using BNE.PessoaJuridica.Domain.Exceptions;
using BNE.PessoaJuridica.WebAPI.Models;
using System.Web.Http.Description;
using System.Web.Script.Serialization;
using BNE.PessoaJuridica.ApplicationService.PessoaJuridica.Command;
using BNE.PessoaJuridica.ApplicationService.PessoaJuridica.Interface;
using log4net;
using Newtonsoft.Json;

namespace BNE.PessoaJuridica.WebAPI.Controllers
{
    [RoutePrefix("api/pessoajuridica/cadastro")]
    public class CadastroController : ApiController
    {

        private readonly IPessoaJuridicaApplicationService _pessoaJuridicaApplicationService;
        private readonly Domain.Services.PessoaJuridicaService _pessoaJuridica;
        private readonly ILog _logger;

        public CadastroController(Domain.Services.PessoaJuridicaService pessoaJuridica, ILog logger, IPessoaJuridicaApplicationService pessoaJuridicaApplicationService)
        {
            _pessoaJuridica = pessoaJuridica;
            _logger = logger;
            _pessoaJuridicaApplicationService = pessoaJuridicaApplicationService;
        }

        #region Cadastrar
        /// <summary>
        /// Efetua cadastro de empresa.
        /// </summary>
        /// <param name="dados">Cadastro Model</param>
        /// <remarks>Insere empresa</remarks>
        /// <response code="200">OK</response>
        /// <response code="400">Bad Request</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost]
        [Route("")]
        [Route("cadastrar")]
        [ApiExplorerSettings()]
        public ResponseMessageResult Post(CadastroEmpresa dados)
        {
            try
            {
                IEnumerable<string> values;
                if (Request.Headers.TryGetValues("BNE_Api_Gateway", out values) && values.First() == "true")
                {
                    dados.IP = "APIGateway";
                }

                if (ModelState.IsValid)
                {
                    var retorno = _pessoaJuridicaApplicationService.CadastrarEmpresa(dados);
                    if (retorno != null)
                    {
                        if (retorno.Error != null)
                        {
                            if (retorno.Error is SemPermissaoParaEditar || retorno.Error is DataDeNascimentoNaoConfere || retorno.Error is CNPJInvalido || retorno.Error is CPFInvalido)
                            {
                                _logger.Warn(JsonConvert.SerializeObject(dados), retorno.Error);
                                return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.BadRequest, retorno.Error.Message));
                            }
                        }
                        return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.Created, retorno));
                    }
              
                }
                else
                {
                    var modelErrors = string.Join(",", ModelState.Values.SelectMany(x => x.Errors).Select(x => string.IsNullOrWhiteSpace(x.ErrorMessage) ? x.Exception.Message : x.ErrorMessage));
                    
                    return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.BadRequest, modelErrors));
                }
            }
            catch (Exception ex)
            {
                if (ex is SemPermissaoParaEditar || ex is DataDeNascimentoNaoConfere || ex is CNPJInvalido || ex is CPFInvalido)
                {
                    _logger.Warn(JsonConvert.SerializeObject(dados), ex);
                    return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message));
                }
                _logger.Error(JsonConvert.SerializeObject(dados), ex);
            }
            throw new HttpResponseException(HttpStatusCode.InternalServerError);
        }
        #endregion

        #region Click2Call
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [Route("Click2Call")]
        public HttpResponseMessage Click2Call(CadastroClick2CallModel dados)
        {
            try
            {
                _pessoaJuridica.Click2Call(dados.Numero, dados.Nome);
                return Request.CreateResponse(HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                _logger.Error(JsonConvert.SerializeObject(dados), ex);
            }
            throw new HttpResponseException(HttpStatusCode.InternalServerError);
        }
        #endregion

    }
}