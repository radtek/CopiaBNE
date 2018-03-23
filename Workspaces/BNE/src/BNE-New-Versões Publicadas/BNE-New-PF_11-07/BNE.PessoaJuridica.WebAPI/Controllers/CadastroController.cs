using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Script.Serialization;
using BNE.Logger.Interface;
using BNE.PessoaJuridica.Domain.Exceptions;
using BNE.PessoaJuridica.WebAPI.Models;
using System.Web.Http.Description;

namespace BNE.PessoaJuridica.WebAPI.Controllers
{
    public class CadastroController : ApiController
    {

        private readonly Domain.PessoaJuridica _pessoaJuridica;
        private readonly Domain.Usuario _usuario;
        private readonly ILogger _logger;
        private readonly Domain.NCall _ncall;

        public CadastroController(Domain.PessoaJuridica pessoaJuridica, Domain.Usuario usuario, Domain.NCall ncall, ILogger logger)
        {
            _pessoaJuridica = pessoaJuridica;
            _usuario = usuario; 
            _logger = logger;
            _ncall = ncall;
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
        [ActionName("cadastrar")]
        [ApiExplorerSettings()]
        public ResponseMessageResult Post(CadastroModel dados)
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
                    var command = AutoMapper.Mapper.Map<CadastroModel, Domain.Command.CriarOuAtualizarPessoaJuridica>(dados);

                    var objUsuario = _pessoaJuridica.SalvarPessoaJuridica(command);

                    if (objUsuario != null)
                    {
                        //TODO Não sei se esse retorno deve estar aqui ou na regra de negócio
                        var retorno = new
                        {
                            linkplanofree = _usuario.GerarHashAcessoLoginAutomatico(objUsuario, "/sala-selecionador"),
                            linkplanopago = _pessoaJuridica.GerarHashCompraDePlano(objUsuario),
                            click2Call = _ncall.FilaBoasVindasDisponivel() || _ncall.FilaCIADisponivel(),
                            numero = command.Usuario.NumeroDDDComercial + command.Usuario.NumeroComercial,
                            nome = command.Usuario.Nome
                        };
                        return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.Created, retorno));
                    }
                }
                else
                {
                    var err = string.Join("</br> ", ModelState.Values.SelectMany(x => x.Errors).Select(x => string.IsNullOrWhiteSpace(x.ErrorMessage) ? x.Exception.Message : x.ErrorMessage));
                    
                    return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.BadRequest, err));
                }
            }
            catch (Exception ex)
            {
                if (ex is SemPermissaoParaEditar || ex is DataDeNascimentoNaoConfere || ex is CNPJInvalido || ex is CPFInvalido)
                {
                    _logger.Warning(ex.Message, new JavaScriptSerializer().Serialize(dados));
                    return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message));
                }

                _logger.Error(ex, string.Empty, new JavaScriptSerializer().Serialize(dados));
            }
            throw new HttpResponseException(HttpStatusCode.InternalServerError);
        }
        #endregion

        #region Put
        // PUT api/pessoajuridica/<controller>/5
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPut]
        public HttpResponseMessage Put(int id, CadastroModel dados)
        {
            if (ModelState.IsValid)
            {
                var command = AutoMapper.Mapper.Map<CadastroModel, Domain.Command.CriarOuAtualizarPessoaJuridica>(dados);
                command.Id = id;
                _pessoaJuridica.SalvarPessoaJuridica(command);
                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
            throw new HttpResponseException(HttpStatusCode.InternalServerError);
        }
        #endregion

        #region Click2Call
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [ActionName("Click2Call")]
        public HttpResponseMessage Click2Call(CadastroClick2CallModel dados)
        {
            try
            {
                _pessoaJuridica.Click2Call(dados.Numero, dados.Nome);
                return Request.CreateResponse(HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, string.Empty, new JavaScriptSerializer().Serialize(dados));
            }
            throw new HttpResponseException(HttpStatusCode.InternalServerError);
        }
        #endregion

        #region Receita
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [ActionName("Receita")]
        public HttpResponseMessage Receita(CadastroReceita dados)
        {
            try
            {
                var command = AutoMapper.Mapper.Map<CadastroReceita, Domain.Command.CriarSolicitacaoReceita>(dados);
                _pessoaJuridica.SalvarSolicitacaoReceita(command);
                return Request.CreateResponse(HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, new JavaScriptSerializer().Serialize(dados));
            }
            throw new HttpResponseException(HttpStatusCode.InternalServerError);
        }
        #endregion

    }
}