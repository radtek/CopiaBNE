using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Results;
using System.Web.Script.Serialization;
using AutoMapper;
using BNE.PessoaJuridica.ApplicationService.PessoaJuridica.Command;
using BNE.PessoaJuridica.Domain.Command;
using BNE.PessoaJuridica.Domain.Exceptions;
using BNE.PessoaJuridica.Domain.Services;
using log4net;

namespace BNE.PessoaJuridica.WebAPI.Controllers
{
    [RoutePrefix("api/pessoajuridica/cadastrousuario")]
    public class CadastroUsuarioController : ApiController
    {
        private readonly ILog _logger;
        private readonly IMapper _mapper;

        private readonly UsuarioService _usuario;
        private readonly UsuarioAdicionalService _usuarioAdicional;

        public CadastroUsuarioController(UsuarioAdicionalService usuarioAdicional, UsuarioService usuario, ILog logger, IMapper mapper)
        {
            _usuario = usuario;
            _usuarioAdicional = usuarioAdicional;

            _logger = logger;
            _mapper = mapper;
        }

        #region Cadastrar

        // POST api/pessoajuridica/<controller>
        [ApiExplorerSettings(IgnoreApi = true)]
        [ActionName("cadastrar")]
        [Route("cadastrar")]
        public ResponseMessageResult Post(CadastroUsuarioEmpresa dados)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var command = _mapper.Map<CadastroUsuarioEmpresa, CriarOuAtualizarUsuarioEmpresa>(dados);

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
                _logger.Error(new JavaScriptSerializer().Serialize(dados), ex);

                if (ex is SemPermissaoParaEditar || ex is DataDeNascimentoNaoConfere)
                {
                    return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message));
                }
            }
            throw new HttpResponseException(HttpStatusCode.InternalServerError);
        }

        #endregion
    }
}