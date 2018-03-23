using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using BNE.Mensagem.Domain.Exceptions;
using BNE.Services.AsyncServices.Base.Plugins.Interface;
using BNE.Services.AsyncServices.Base.ProcessosAssincronos;

namespace BNE.Mensagem.AsyncExecutor.Plugins.PluginsEntrada
{
    [Export(typeof(IInputPlugin))]
    [ExportMetadata("Type", "EnvioEmail")]
    public class EnvioEmail : Plugins.InputPlugin
    {

        #region DoExecuteTask
        protected override IPluginResult DoExecuteTask(ParametroExecucaoCollection objParametros, Dictionary<string, byte[]> objAnexos)
        {
            var email = DomainEmail;

            var idEmail = objParametros["idEmail"].ValorInt;

            if (idEmail != null)
            {
                var objEmail = email.RecuperarEmail(idEmail.Value);

                var emailRemetente = objEmail.EmailRemetente;
                var emailDestinatario = objEmail.EmailDestinatario;
                var assunto = email.Assunto(objEmail);
                var mensagem = email.HTML(objEmail);

                try
                {
                    var objMensagemEmail = new OutputResult.Mensagem.MensagemEmail
                    {
                        Assunto = assunto,
                        Descricao = mensagem,
                        From = emailRemetente,
                        To = emailDestinatario,
                        IdMensagem = idEmail.Value,
                    };

                    return new OutputResult.Mensagem(this, new List<OutputResult.Mensagem.MensagemEmail> { objMensagemEmail }, false);
                }
                catch (Exception ex)
                {
                    Core.LogError(ex);
                }
            }

            return new OutputResult.Mensagem(this, true);
        }
        #endregion

    }
}
