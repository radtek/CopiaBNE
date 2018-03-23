using System.Collections.Generic;
using BNE.BLL.AsyncServices;

namespace BNE.BLL.MailService
{
    public sealed class SendgridController
    {
        private static Providers.SendGrid _instance;

        private static readonly string SendgridApiKeyToken = Parametro.RecuperaValorParametro(AsyncServices.Enumeradores.Parametro.SendgridAPIKeyToken);
        private static readonly string ContaPadraoEnvioEmail = Parametro.RecuperaValorParametro(AsyncServices.Enumeradores.Parametro.ContaPadraoEnvioEmail);

        private static Providers.SendGrid Instance => _instance ?? (_instance = new Providers.SendGrid(SendgridApiKeyToken, ContaPadraoEnvioEmail));

        public static void Send(string emailRemetente, string emailDestinatario, string assunto, string mensagem, Dictionary<string, byte[]> attachments = null, List<string> tags = null, int? timeoutMs = null)
        {
            Instance.Send(emailRemetente, emailDestinatario, assunto, mensagem, attachments, tags, timeoutMs);
        }
    }
}