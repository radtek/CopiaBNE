using BNE.BLL.AsyncServices;
using BNE.BLL.MailService.Providers;

namespace BNE.BLL.MailService
{
    public sealed class AmazonSESController
    {
        private static AmazonSES _instance;

        private static readonly string AccessKeyId = Parametro.RecuperaValorParametro(AsyncServices.Enumeradores.Parametro.AmazonAccessKeyId);
        private static readonly string SecretKey = Parametro.RecuperaValorParametro(AsyncServices.Enumeradores.Parametro.AmazonSecretKey);
        private static readonly string ProfileName = Parametro.RecuperaValorParametro(AsyncServices.Enumeradores.Parametro.AmazonProfileName);

        private static AmazonSES Instance => _instance ?? (_instance = new AmazonSES(ProfileName, AccessKeyId, SecretKey));

        public static void Send(string emailRemetente, string emailDestinatario, string assunto, string mensagem)
        {
            Instance.Send(emailRemetente, emailDestinatario, assunto, mensagem);
        }
    }
}