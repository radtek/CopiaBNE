using System;
using System.Collections.Generic;

namespace BNE.BLL.Custom
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

            var destinatarios = new List<Mandrill.EmailAddress> { new Mandrill.EmailAddress { email = emailDestinatario, name = nomeDestinatario } };
            var message = new Mandrill.EmailMessage
            {
                to = destinatarios,
                from_name = nomeRemetente,
                from_email = emailRemetente,
                subject = assunto,
                html = mensagem,
                auto_text = true
            };

            #endregion

            if (!string.IsNullOrWhiteSpace(apiKey))
            {
                try
                {
                    Mandrill.MandrillApi api;
                    if (timeoutMs.HasValue && timeoutMs.Value > 0)
                    {
                        api = new Mandrill.MandrillApi(apiKey, true, timeoutMs.Value);
                    }
                    else
                    {
                        api = new Mandrill.MandrillApi(apiKey);
                    }
                    var task = api.SendMessageAsync(message);

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
                    EL.GerenciadorException.GravarExcecao(ex);
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
