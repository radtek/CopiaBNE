using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Script.Serialization;
using AutoMapper;
using BNE.Logger.Interface;
using BNE.PessoaJuridica.Domain.Exceptions;
using BNE.PessoaJuridica.WebAPI.Models;
using System.Web.Http.Description;

namespace BNE.PessoaJuridica.WebAPI.Controllers
{
    public class CadastroUsuarioController : ApiController
    {

        private readonly Domain.Usuario _usuario;
        private readonly Domain.UsuarioAdicional _usuarioAdicional;
        private readonly ILogger _logger;

        public CadastroUsuarioController(Domain.UsuarioAdicional usuarioAdicional, Domain.Usuario usuario, ILogger logger)
        {
            _usuario = usuario;
            _usuarioAdicional = usuarioAdicional;

            _logger = logger;
        }

        #region Cadastrar
        // POST api/pessoajuridica/<controller>
        [ApiExplorerSettings(IgnoreApi = true)]
        [ActionName("cadastrar")]
        public ResponseMessageResult Post(CadastroUsuarioModel dados)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var command = Mapper.Map<CadastroUsuarioModel, Domain.Command.CriarOuAtualizarUsuarioEmpresa>(dados);

                    var objUsuario = _usuarioAdicional.SalvarUsuarioAdicional(command);
                    if (objUsuario != null)
                    {
                        return ResponseMessage(new HttpResponseMessage(HttpStatusCode.Created)
                        {
                            RequestMessage = Request,
                            Content = new StringContent(_usuario.GerarHashAcessoLoginAutomatico(objUsuario, "/sala-selecionador"))
                        });
                    }
                }
                else
                {
                    var errors = new Dictionary<string, IEnumerable<string>>();
                    foreach (var keyValue in ModelState)
                    {
                        errors[keyValue.Key] = keyValue.Value.Errors.Select(e => string.IsNullOrWhiteSpace(e.ErrorMessage) ? e.Exception.Message : e.ErrorMessage);
                    }
                    return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.BadRequest, errors));
                }
            }
            catch (Exception ex)
            {
                if (ex is SemPermissaoParaEditar || ex is DataDeNascimentoNaoConfere)
                {
                    _logger.Warning(ex.Message, new JavaScriptSerializer().Serialize(dados));
                    return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message));
                }

                _logger.Error(ex, string.Empty, new JavaScriptSerializer().Serialize(dados));
            }
            throw new HttpResponseException(HttpStatusCode.InternalServerError);
        }
        #endregion

    }
}