using System;
using System.Collections.Generic;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;

namespace BNE.BLL.MailService.Providers
{
    public class AmazonSES
    {
        private readonly IAmazonSimpleEmailService _client;
       

        public AmazonSES(string profileName, string accessKeyId, string secretKey)
        {
            if (_client == null)
            {
                Amazon.Util.ProfileManager.RegisterProfile(profileName, accessKeyId, secretKey);
                _client = new AmazonSimpleEmailServiceClient(Amazon.RegionEndpoint.USWest2);
            }
        }

        public bool Send(string emailRemetente, string emailDestinatario, string assunto, string mensagem)
        {
            var subject = new Content(assunto);
            var message = new Message(subject, new Body { Html = new Content(mensagem) });
            var destinatarios = new Destination { ToAddresses = new List<string> { emailDestinatario } };

            SendEmailRequest request = new SendEmailRequest(emailRemetente, destinatarios, message);

            try
            {
                _client.SendEmail(request);

                return true;

            }
            catch (Exception ex)
            {
                BNE.EL.GerenciadorException.GravarExcecao(ex);
            }
            return false;
        }
    }
}
