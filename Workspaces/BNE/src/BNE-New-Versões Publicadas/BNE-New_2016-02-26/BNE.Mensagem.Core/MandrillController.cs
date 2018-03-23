using System;
using System.Collections.Generic;
using BNE.Mensagem.AsyncServices.BLL;
using Mandrill.Models;
using Enumeradores = BNE.Mensagem.AsyncServices.BLL.Enumeradores;

namespace BNE.Mensagem.Core
{
    public static class MandrillController
    {
        
        #region Métodos

        #region Send
        public static void Send(string emailDestinatario, string nomeDestinatario, string assunto, string mensagem, string emailRemetente, string nomeRemetente, int? timeoutMs = null)
        {
            var parametros = new List<Enumeradores.Parametro>
                {
                    Enumeradores.Parametro.MandrillApiKey
                };
            var dicionarioParametros = Parametro.ListarParametros(parametros);

            var apiKey = dicionarioParametros[Enumeradores.Parametro.MandrillApiKey];

            #region Propriedades

            var destinatarios = new List<EmailAddress> { new EmailAddress { Email = emailDestinatario, Name = nomeDestinatario } };
            var message = new Mandrill.Requests.Messages.SendMessageRequest(new EmailMessage
            {
                To = destinatarios,
                FromName = nomeRemetente,
                FromEmail = emailRemetente,
                Subject = assunto,
                Html = mensagem,
                AutoText = true
            });

            #endregion

            if (!string.IsNullOrWhiteSpace(apiKey))
            {
                try
                {
                    Mandrill.MandrillApi api;
                    if (timeoutMs.HasValue && timeoutMs.Value > 0)
                    {
                        api = new Mandrill.MandrillApi(apiKey);
                    }
                    else
                    {
                        api = new Mandrill.MandrillApi(apiKey);
                    }
                    var task = api.SendMessage(message);

                    if (timeoutMs.HasValue && timeoutMs.Value > 0)
                    {
                        var retorno = task.Wait(timeoutMs.Value);
                        if (!retorno)
                            throw new TimeoutException("Mandrill - Timed out");
                    }
                    else
                        task.Wait(30000);
                }
                catch (Exception ex)
                {
                    new Services.AsyncServices.Base.Autofac().Logger.Error(ex);
                }
            }
            else
            {
                throw new Exception("API key não configurada!");
            }
        }
        #endregion

        #endregion

    }
}
