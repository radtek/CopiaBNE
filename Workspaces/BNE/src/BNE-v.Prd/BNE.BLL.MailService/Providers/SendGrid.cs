using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using BNE.EL;
using SendGrid;

namespace BNE.BLL.MailService.Providers
{
    public class SendGrid
    {
        private readonly Web _client;
        private readonly string _contaPadraoEnvio;

        public SendGrid(string apiKey, string contaPadraoEnvio)
        {
            ServicePointManager.DefaultConnectionLimit = 10000;
            ServicePointManager.Expect100Continue = false;

            _client = new Web(apiKey, null, TimeSpan.FromMinutes(30));
            
            _contaPadraoEnvio = contaPadraoEnvio;
        }

        public void Send(string emailRemetente, string emailDestinatario, string assunto, string mensagem,
            Dictionary<string, byte[]> attachments = null, List<string> tags = null, int? timeoutMs = null)
        {
            emailRemetente = emailRemetente.Trim();
            if (string.IsNullOrEmpty(emailRemetente))
            {
                emailRemetente = _contaPadraoEnvio;
            }

            //Tratando Subject para evitar assuntos inválidos (com quebra de linhas).
            assunto = assunto.Replace('\r', ' ').Replace('\n', ' ');
            emailDestinatario = emailDestinatario.Replace(" ", "").Replace(";", "; ");

            #region Propriedades

            var from = new MailAddress(emailRemetente);
            var to = emailDestinatario.Split(';').Select(x => new MailAddress(x));

            var sendgridMessage = new SendGridMessage(from, to.ToArray(), assunto, mensagem, string.Empty)
            {
                ReplyTo = new List<MailAddress> { new MailAddress(emailRemetente) }.ToArray(),
                //Ajuste para o BNE. Manda o remetente no reply to. E usamos uma conta do BNE como sender do e-mail
                From = new MailAddress(_contaPadraoEnvio, emailRemetente)
            };
            sendgridMessage.EnableOpenTracking();
            sendgridMessage.EnableClickTracking();
            sendgridMessage.DisableUnsubscribe();
            sendgridMessage.SetCategories(tags);

            //Adicionando anexos
            if (attachments != null && !attachments.Equals(default(Dictionary<string, byte[]>)))
            {
                sendgridMessage.StreamedAttachments = attachments.ToDictionary(d => d.Key,
                    d => new MemoryStream(d.Value));
            }

            #endregion

            try
            {
                _client.DeliverAsync(sendgridMessage).Wait();
            }
            catch (TaskCanceledException ex)
            {
                GerenciadorException.GravarExcecao(ex, "TaskCanceledException");
                throw;
            }
            catch (Exception ex)
            {
                GerenciadorException.GravarExcecao(ex);
                throw;
            }
        }
    }
}