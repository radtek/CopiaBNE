using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SendGrid;
using System.Net.Mail;
using System.IO;
using System.Net;
using BNE.Mensagem.AsyncServices.BLL;
using Enumeradores = BNE.Mensagem.AsyncServices.BLL.Enumeradores;

namespace BNE.Mensagem.Core
{
    public sealed class SendGridController
    {
        static SendgridControllerService _instance;

        private static SendgridControllerService Instance
        {
            get
            {
                return _instance ?? (_instance = new SendgridControllerService());
            }
        }

        public static void Send(string emailRemetente, string nomeRemetente, string emailDestinatario, string nomeDestinatario, string assunto, string mensagem, Dictionary<string, byte[]> attachments = null, List<string> tags = null, int? timeoutMs = null)
        {
            Instance.Send(emailRemetente, nomeRemetente, emailDestinatario, nomeDestinatario, assunto, mensagem, attachments, tags, timeoutMs);
        }
    }

    internal class SendgridControllerService
    {

        readonly Web _client;

        public SendgridControllerService()
        {
            ServicePointManager.DefaultConnectionLimit = 100;
            ServicePointManager.Expect100Continue = false;

            _client = new Web(APIKey(), null, TimeSpan.FromMinutes(30));
        }

        private string APIKey()
        {
            var parametros = new List<Enumeradores.Parametro>
                {
                    Enumeradores.Parametro.SendgridAPIKeyToken
                };
            var dicionarioParametros = Parametro.ListarParametros(parametros);

            var apiKey = dicionarioParametros[Enumeradores.Parametro.SendgridAPIKeyToken];

            if (!string.IsNullOrWhiteSpace(apiKey))
            {
                return apiKey;
            }
            throw new Exception("Sendgrid api key não configurada!");
        }

        private string ContaPadraoEnvio()
        {
            var parametros = new List<Enumeradores.Parametro>
                {
                    Enumeradores.Parametro.ContaPadraoEnvioEmail
                };
            var dicionarioParametros = Parametro.ListarParametros(parametros);

            var apiKey = dicionarioParametros[Enumeradores.Parametro.ContaPadraoEnvioEmail];

            if (!string.IsNullOrWhiteSpace(apiKey))
            {
                return apiKey;
            }
            throw new Exception("Conta padrão de envio não configurada!");
        }

        #region Métodos

        #region Send
        public void Send(string emailRemetente, string nomeRemetente, string emailDestinatario, string nomeDestinatario, string assunto, string mensagem, Dictionary<string, byte[]> attachments = null, List<string> tags = null, int? timeoutMs = null)
        {
            emailRemetente = emailRemetente.Trim();
            if (String.IsNullOrEmpty(emailRemetente))
            {
                emailRemetente = ContaPadraoEnvio();
            }

            //Tratando Subject para evitar assuntos inválidos (com quebra de linhas).
            assunto = assunto.Replace('\r', ' ').Replace('\n', ' ');
            emailDestinatario = emailDestinatario.Replace(" ", "").Replace(";", "; ");

            #region Propriedades
            var from = new MailAddress(emailRemetente, nomeRemetente);
            var to = new List<MailAddress> { new MailAddress(emailDestinatario, nomeDestinatario) };

            var sendgridMessage = new SendGridMessage(from, to.ToArray(), assunto, mensagem, string.Empty)
            {
                ReplyTo = new List<MailAddress> { new MailAddress(emailRemetente) }.ToArray(), //Ajuste para o BNE. Manda o remetente no reply to. E usamos uma conta do BNE como sender do e-mail
                From = new MailAddress(ContaPadraoEnvio(), emailRemetente)
            };
            sendgridMessage.EnableOpenTracking();
            sendgridMessage.EnableClickTracking();
            sendgridMessage.DisableUnsubscribe();
            sendgridMessage.SetCategories(tags);

            //Adicionando anexos
            if (attachments != null && !attachments.Equals(default(Dictionary<string, byte[]>)))
            {
                sendgridMessage.StreamedAttachments = attachments.ToDictionary(d => d.Key, d => new MemoryStream(d.Value));
            }
            #endregion

            try
            {
                _client.DeliverAsync(sendgridMessage).Wait();
            }
            catch (TaskCanceledException ex)
            {
                new Services.AsyncServices.Base.Autofac().Logger.Error(ex);
            }
            catch (Exception ex)
            {
                new Services.AsyncServices.Base.Autofac().Logger.Error(ex);
                throw;
            }
        }
        #endregion

        #endregion

    }
}
