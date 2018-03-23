using System;
using System.Collections.Generic;
using BNE.BLL.AsyncServices;
using BNE.BLL.MailService.Providers;

namespace BNE.BLL.MailService
{
    public sealed class SMTPCloudController
    {
        public static readonly string SmtpServer = Parametro.RecuperaValorParametro(AsyncServices.Enumeradores.Parametro.SMTPCloudServer);
        public static readonly int SmtpPort = Convert.ToInt32(Parametro.RecuperaValorParametro(AsyncServices.Enumeradores.Parametro.SMTPCloudPort));
        public static readonly string ContaPadraoEnvioEmail = Parametro.RecuperaValorParametro(AsyncServices.Enumeradores.Parametro.ContaPadraoEnvioEmail);

        public static void Send(string emailRemetente, string emailDestinatario, string assunto, string mensagem, Dictionary<string, byte[]> attachments = null, List<string> tags = null, int? timeoutMs = null)
        {
            new SMTP(SmtpServer, SmtpPort, ContaPadraoEnvioEmail).Send(emailRemetente, emailDestinatario, assunto, mensagem, attachments, tags, timeoutMs);
        }
    }
}