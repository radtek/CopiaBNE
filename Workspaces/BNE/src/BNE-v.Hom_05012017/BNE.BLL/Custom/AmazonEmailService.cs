using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;

namespace BNE.BLL.Custom
{
   public class AmazonEmailService
    {
        #region Atributos
        private string _accessKeyId = Parametro.RecuperaValorParametro(Enumeradores.Parametro.AmazonAccessKeyId);
        private string _secretKey = Parametro.RecuperaValorParametro(Enumeradores.Parametro.AmazonSecretKey);
        private string _profileName = System.Configuration.ConfigurationManager.AppSettings["AWSProfileName"].ToString();
        IAmazonSimpleEmailService client;
        #endregion

        #region Propriedades
        public bool PermiteEnvioEmail
        {
            get
            {
                return PermiteEnvio();
            }
        }
        #endregion

       #region Construtor
        public AmazonEmailService()
        {

            if (client == null)
            {
                Amazon.Util.ProfileManager.RegisterProfile(profileName: _profileName, accessKeyId: _accessKeyId, secretKey: _secretKey);
                Amazon.RegionEndpoint REGION = Amazon.RegionEndpoint.USWest2;
                client = new AmazonSimpleEmailServiceClient(REGION);
            }

        }
        #endregion

        #region Enviar
        /// <summary>
        /// Envia e-mail via amazon
        /// </summary>
        /// <param name="emailDestinatario"></param>
        /// <param name="emailRemetente"></param>
        /// <param name="assunto"></param>
        /// <param name="mensagem"></param>
        public bool EnviarEmail(string emailDestinatario, string emailRemetente, string assunto, string mensagem)
        {

            if (!PermiteEnvioEmail)
                return false;


            if (VerificarEmail(ref emailRemetente, emailDestinatario))
            {
                var subject = new Content(assunto);
                Body body = new Body();
                body.Html = new Content(mensagem);

                var destinatarios = new Destination { ToAddresses = new List<string> { emailDestinatario } };

                var message = new Message(subject, body);

                SendEmailRequest request = new SendEmailRequest(emailRemetente, destinatarios, message);

                int tries = 0;

                while (tries <= 3)
                {
                    try
                    {
                        client.SendEmail(request);

                        return true;

                    }
                    catch (Exception ex)
                    {
                        if (ex.InnerException.Message.Equals("Rate exceeded"))
                        {
                            tries++;

                            if (tries == 3)
                                EL.GerenciadorException.GravarExcecao(ex, string.Format("Erro ao Enviar E-Mail Varios Destinatários via Amazon, tentativas {0}", tries));

                            Thread.Sleep(1000);

                            continue;
                        }
                        else
                        {
                            EL.GerenciadorException.GravarExcecao(ex, "Erro ao Enviar E-Mail Varios Destinatários via Amazon");
                            return false;
                        }
                    }
                }

            }

            return false;
        }
        #endregion

        #region EnviarEmailsVariosDestinatarios
        /// <summary>
        /// Envia e-mails vários destinatários em cópia
        /// </summary>
        /// <param name="emailDestinatarios"></param>
        /// <param name="emailRemetente"></param>
        /// <param name="assunto"></param>
        /// <param name="mensagem"></param>
        public bool EnviarEmailsVariosDestinatarios(List<string> emailDestinatarios, string emailRemetente, string assunto, string mensagem)
        {

            if (!PermiteEnvioEmail)
                return false;

            var subject = new Content(assunto);
            Body body = new Body();
            body.Html = new Content(mensagem);

            List<string> EmailsTo = new List<string>();

            foreach (var email in emailDestinatarios)
            {
                if (!EmailsTo.Contains(email))
                {
                    EmailsTo.Add(email);
                }
            }

            var destinatarios = new Destination { BccAddresses = EmailsTo };

            var message = new Message(subject, body);

            SendEmailRequest request = new SendEmailRequest(emailRemetente, destinatarios, message);

            int tries = 0;

            while (tries <= 3)
            {
                try
                {
                    client.SendEmail(request);

                    return true;
                }
                catch (Exception ex)
                {
                    if (ex.InnerException.Message.Equals("Rate exceeded"))
                    {
                        tries++;

                        if (tries == 3)
                            EL.GerenciadorException.GravarExcecao(ex, string.Format("Erro ao Enviar E-Mail via Amazon, tentativas: {0}", tries));

                        Thread.Sleep(1000);

                        continue;
                    }
                    else
                    {
                        EL.GerenciadorException.GravarExcecao(ex, "Erro ao Enviar E-Mail via Amazon");
                        return false;
                    }

                }
            }

            return false;
        }
        #endregion



        #region VerificarQuota
        /// <summary>
        /// Verifica se a Cota diária de emails foi atingida.
        /// </summary>
        /// <returns>verdadeiro se a cota permite envio ou falso caso contrário</returns>
        public bool PermiteEnvio()
        {
            int tries = 0;
            while (tries <= 3)
            {
                try
                {
                    var quota = client.GetSendQuota();

                    if (quota.Max24HourSend - quota.SentLast24Hours > 0)
                        return true;

                    return false;
                }
                catch (Exception ex)
                {
                    if (ex.InnerException.Message.Equals("Rate exceeded"))
                    {
                        tries++;

                        if (tries == 3)
                            EL.GerenciadorException.GravarExcecao(ex, string.Format("Erro ao Recuperar quota da Amazon Tentativas: {0}", tries));

                        Thread.Sleep(1000);
                        continue;
                    }
                    else
                    {
                        EL.GerenciadorException.GravarExcecao(ex, "Erro ao Recuperar quota da Amazon");
                        return false;
                    }

                }

            }

            return false;

        }
        #endregion

        #region VerificarEmail
        public static bool VerificarEmail(ref string from, string to)
        {
            bool emailValido = true;

            if (string.IsNullOrEmpty(from.Trim()))
            {
                from = Parametro.RecuperaValorParametro(Enumeradores.Parametro.ContaPadraoEnvioEmail);
            }

            if (!Validacao.ValidarEmail(from))
            {
                emailValido = false;
            }

            if (string.IsNullOrEmpty(to))
            {
                emailValido = false;
            }
            else if (!Validacao.ValidarEmail(to))
            {
                emailValido = false;
            }

            return emailValido;
        }
        #endregion

    }
}
