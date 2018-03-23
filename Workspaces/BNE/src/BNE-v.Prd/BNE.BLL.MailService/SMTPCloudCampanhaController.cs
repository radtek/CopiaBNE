using System;
using System.Collections.Generic;
using BNE.BLL.AsyncServices;
using BNE.BLL.MailService.Providers;

namespace BNE.BLL.MailService
{
    public sealed class SMTPCloudCampanhaController
    {
        public static readonly string SmtpServer = Parametro.RecuperaValorParametro(BLL.AsyncServices.Enumeradores.Parametro.SMTPCloudCampanhaServer);
        public static readonly int SmtpPort = Convert.ToInt32(Parametro.RecuperaValorParametro(BLL.AsyncServices.Enumeradores.Parametro.SMTPCloudCampanhaPort));
        public static readonly string ContaPadraoEnvioEmail = Parametro.RecuperaValorParametro(BLL.AsyncServices.Enumeradores.Parametro.ContaPadraoEnvioEmail);

        public static void Send(string emailRemetente, string emailDestinatario, string assunto, string mensagem, Dictionary<string, byte[]> attachments = null, List<string> tags = null, int? timeoutMs = null)
        {
            new SMTP(SmtpServer, SmtpPort, ContaPadraoEnvioEmail).Send(emailRemetente, emailDestinatario, assunto, mensagem, attachments, tags, timeoutMs);
        }
    }
}