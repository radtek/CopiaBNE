using System;
using System.Dynamic;
using BNE.Data.Infrastructure;
using BNE.ExceptionLog.Interface;
using BNE.Mensagem.AsyncServices.BLL;
using BNE.Mensagem.Data.Repositories;
using BNE.Mensagem.Domain.Exceptions;
using BNE.Services.AsyncServices.Base.ProcessosAssincronos;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using ProcessoAssincrono = BNE.Mensagem.AsyncServices.Base.ProcessosAssincronos.ProcessoAssincrono;

namespace BNE.Mensagem.Domain
{
    public class Email
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly TemplateEmail _templateEmail;
        private readonly Sistema _objSistema;
        private readonly Status _objStatus;
        private readonly Usuario _objUsuario;
        private readonly IEmailRepository _emailRepository;

        public Email(TemplateEmail templateEmail, Sistema objSistema, Status objStatus, Usuario objUsuario, IEmailRepository emailRepository, ILogger logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _templateEmail = templateEmail;
            _objSistema = objSistema;
            _emailRepository = emailRepository;
            _objStatus = objStatus;
            _objUsuario = objUsuario;
        }

        public Model.Email RecuperarEmail(int identificador)
        {
            return _emailRepository.GetById(identificador);
        }

        public bool EnviarEmail(Command.EnviarEmail objEnviarEmail)
        {
            try
            {
                var objSistema = _objSistema.RecuperarSistema(objEnviarEmail.Sistema);
                var objTemplate = _templateEmail.RecuperarTemplate(objSistema, objEnviarEmail.Template);

                if (objTemplate == null)
                    throw new TemplateNaoEncontradoException(objEnviarEmail.Template);

                //TODO Falta implementar o anexo

                Model.Usuario objUsuarioRemetente = null;
                if (objEnviarEmail.GuidUsuarioRemetente != null)
                    objUsuarioRemetente = _objUsuario.RecuperarUsuario(objSistema, Guid.Parse(objEnviarEmail.GuidUsuarioRemetente));

                Model.Usuario objUsuarioDestino = null;
                if (objEnviarEmail.GuidUsuarioDestino != null)
                    objUsuarioDestino = _objUsuario.RecuperarUsuario(objSistema, Guid.Parse(objEnviarEmail.GuidUsuarioDestino));

                var objEmail = new Model.Email
                {
                    DataCadastro = DateTime.Now,
                    EmailDestinatario = objEnviarEmail.EmailDestino,
                    EmailRemetente = objEnviarEmail.EmailRemetente,
                    Parametros = objEnviarEmail.Parametros != null ? JsonConvert.SerializeObject(objEnviarEmail.Parametros) : null,
                    TemplateEmail = objTemplate,
                    Status = _objStatus.RecuperarStatus(Enumeradores.Status.NaoEnviado),
                    UsuarioRemetente = objUsuarioRemetente,
                    UsuarioDestinatario = objUsuarioDestino
                };

                //Testa se possui todos os parametros
                Assunto(objEmail);
                HTML(objEmail);

                _emailRepository.Add(objEmail);

                _unitOfWork.Commit();

                try
                {
                    var parametrosAtividade = new ParametroExecucaoCollection
                    {
                        {"idEmail", "idEmail", objEmail.Id.ToString(), objEmail.Id.ToString()}
                    };

                    ProcessoAssincrono.IniciarAtividade(
                        AsyncServices.BLL.Enumeradores.TipoAtividade.Email,
                        objSistema,
                        objTemplate,
                        PluginsCompatibilidade.CarregarPorMetadata("EnvioEmail", "PluginSaidaEmail"),
                        parametrosAtividade,
                        null);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                }

                return true;

            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (ex is SemParametroException || ex is TemplateNaoEncontradoException)
                    throw;
            }
            return false;
        }

        public string HTML(Model.Email objEmail)
        {
            if (objEmail.Parametros != null)
            {
                dynamic parametros = JsonConvert.DeserializeObject<ExpandoObject>(objEmail.Parametros, new ExpandoObjectConverter());
                return _templateEmail.HTML(objEmail.TemplateEmail, parametros);
            }
            return _templateEmail.HTML(objEmail.TemplateEmail, null);
        }

        public string Assunto(Model.Email objEmail)
        {
            if (objEmail.Parametros != null)
            {
                dynamic parametros = JsonConvert.DeserializeObject<ExpandoObject>(objEmail.Parametros, new ExpandoObjectConverter());
                return _templateEmail.Assunto(objEmail.TemplateEmail, parametros);
            }
            return _templateEmail.Assunto(objEmail.TemplateEmail, null);
        }

        public void AtualizarDataEnvio(Model.Email objEmail)
        {
            objEmail.Status = _objStatus.RecuperarStatus(Enumeradores.Status.Enviado);
            objEmail.DataEnvio = DateTime.Now;

            _emailRepository.Update(objEmail);

            _unitOfWork.Commit();
        }

        public void AtualizarErro(Model.Email objEmail)
        {
            objEmail.Status = _objStatus.RecuperarStatus(Enumeradores.Status.Erro);

            _emailRepository.Update(objEmail);

            _unitOfWork.Commit();
        }

        public void AtualizarErroFaltaParametro(Model.Email objEmail)
        {
            objEmail.Status = _objStatus.RecuperarStatus(Enumeradores.Status.FaltaParametro);

            _emailRepository.Update(objEmail);

            _unitOfWork.Commit();
        }

    }
}
