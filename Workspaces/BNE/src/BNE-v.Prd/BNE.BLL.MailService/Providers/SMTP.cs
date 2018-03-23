using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using BNE.EL;

namespace BNE.BLL.MailService.Providers
{
    public class SMTP
    {
        private readonly SmtpClient _client;
        private readonly string _contaPadraoEnvioEmail;

        public SMTP(string smtpServer, int smptPort, string contaPadraoEnvioEmail)
        {
            ServicePointManager.DefaultConnectionLimit = 10000;
            ServicePointManager.Expect100Continue = false;

            _client = new SmtpClient(smtpServer, smptPort);
            _contaPadraoEnvioEmail = contaPadraoEnvioEmail;
        }

        public void Send(string emailRemetente, string emailDestinatario, string assunto, string mensagem,
            Dictionary<string, byte[]> attachments = null, List<string> tags = null, int? timeoutMs = null)
        {
            emailRemetente = emailRemetente.Trim();
            if (string.IsNullOrEmpty(emailRemetente))
            {
                emailRemetente = _contaPadraoEnvioEmail;
            }

            //Tratando Subject para evitar assuntos inválidos (com quebra de linhas).
            assunto = assunto.Replace('\r', ' ').Replace('\n', ' ');
            emailDestinatario = emailDestinatario.Replace(" ", "").Replace(";", "; ");

            #region Propriedades

            var from = new MailAddress(_contaPadraoEnvioEmail);
            var to = emailDestinatario.Split(';').Select(x => new MailAddress(x));

            var mail = new MailMessage
            {
                IsBodyHtml = true,
                BodyTransferEncoding = TransferEncoding.QuotedPrintable,
                From = from,
                Subject = assunto,
                Body = mensagem
            };

            //Ajuste para o BNE. Manda o remetente no reply to. E usamos uma conta do BNE como sender do e-mail
            mail.ReplyToList.Add(new MailAddress(emailRemetente));
            mail.Headers.Add("Message-Id", string.Format("<{0}@{1}>", Guid.NewGuid(), _client.Host));

            foreach (var address in to)
            {
                mail.To.Add(address);
            }

            //Adicionando anexos
            if (attachments != null && !attachments.Equals(default(Dictionary<string, byte[]>)))
            {
                foreach (var attachment in attachments)
                {
                    mail.Attachments.Add(new Attachment(new MemoryStream(attachment.Value), attachment.Key));
                }
            }

            #endregion

            try
            {
                _client.Send(mail);
            }
            catch (Exception ex)
            {
                GerenciadorException.GravarExcecao(ex);
                throw;
            }
        }

    }
}