using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using System.Net;
using System.IO;
using System.Collections.ObjectModel;

namespace BNE.Services.Base.Mailling
{
    /// <summary>
    /// Classe para serviços de email
    /// </summary>
    public static class MailService
    {

        #region Propriedades

        #region defaultConfiguration
        /// <summary>
        /// A configuração padrão do servidor de email
        /// </summary>
        private static MailServiceConfiguration _defaultConfiguration = null;
        /// <summary>
        /// A configuração padrão do servidor de email
        /// </summary>
        private static MailServiceConfiguration defaultConfiguration
        {
            get
            {
                _defaultConfiguration = new MailServiceConfiguration();
                _defaultConfiguration.SmtpHost = Dicionario.RecuperaValorDicionario(Employer.Plataforma.BLL.Enumeradores.ItemDicionario.ServidorSMTPPadrao);
                _defaultConfiguration.SmtpUser = Dicionario.RecuperaValorDicionario(Employer.Plataforma.BLL.Enumeradores.ItemDicionario.UsuarioSMTPPadrao);
                _defaultConfiguration.SmtpPassword = Dicionario.RecuperaValorDicionario(Employer.Plataforma.BLL.Enumeradores.ItemDicionario.SenhaSMTPPadrao);
                
                return _defaultConfiguration;
            }
        }

        #endregion

        #endregion

        #region SendEmail
        /// <summary>
        /// Envia um email
        /// </summary>
        /// <param name="configuration">A configuração do smtp</param>
        /// <param name="from">O remetente</param>
        /// <param name="to">O destinatário</param>
        /// <param name="subject">O assunto</param>
        /// <param name="message">A mensagem</param>
        /// <param name="attachments">Anexos</param>
        /// <returns>True em caso de sucesso</returns>
        public static Boolean SendEmail(MailServiceConfiguration configuration, String from, String to, String subject, String message, Dictionary<String, byte[]> attachments)
        {
            // Monta a mensagem
            using (MailMessage objMailMessage = new MailMessage(from, to))
            {
                Collection<MemoryStream> colMS = new Collection<MemoryStream>();
                objMailMessage.IsBodyHtml = true;
                objMailMessage.BodyEncoding = Encoding.UTF8;
                objMailMessage.Subject = subject;
                objMailMessage.Body = message;

                // Anexos
                if (attachments != null)
                {
                    foreach (String key in attachments.Keys)
                    {
                        MemoryStream objMemoryStream = new MemoryStream(attachments[key]);
                        colMS.Add(objMemoryStream);
                        objMailMessage.Attachments.Add(new Attachment(objMemoryStream, key));
                    }
                }

                // Cria o client que vai enviar a mensagem
                SmtpClient objSmtClient = new SmtpClient();
                objSmtClient.Host = configuration.SmtpHost;
                objSmtClient.Port = 25;
                objSmtClient.Credentials = new NetworkCredential(configuration.SmtpUser, configuration.SmtpPassword);

                try
                {
                    objSmtClient.Send(objMailMessage);
                    return true;
                }
                catch (Exception ex)
                {
                    PlataformaLog.LogError.WriteLog(ex);
                    return false;
                }
                finally
                {
                    if (colMS.Count > 0)
                    {
                        foreach (MemoryStream ms in colMS)
                        {
                            ms.Dispose();
                        }
                    }
                }
            }

        }
        /// <summary>
        /// Envia um email
        /// </summary>
        /// <param name="configuration">A configuração do smtp</param>
        /// <param name="from">O remetente</param>
        /// <param name="to">O destinatário</param>
        /// <param name="subject">O assunto</param>
        /// <param name="message">A mensagem</param>        
        /// <returns>True em caso de sucesso</returns>
        public static Boolean SendEmail(MailServiceConfiguration configuration, String from, String to, String subject, String message)
        {
            return SendEmail(configuration, from, to, subject, message, null);
        }
        /// <summary>
        /// Envia um email con a configuração de smtp padrão
        /// </summary>        
        /// <param name="from">O remetente</param>
        /// <param name="to">O destinatário</param>
        /// <param name="subject">O assunto</param>
        /// <param name="message">A mensagem</param>
        /// <param name="attachments">Anexos</param>
        /// <returns>True em caso de sucesso</returns>
        public static Boolean SendEmail(String from, String to, String subject, String message, Dictionary<String, byte[]> attachments)
        {
            return SendEmail(defaultConfiguration, from, to, subject, message, attachments);
        }
        /// <summary>
        /// Envia um email con a configuração de smtp padrão
        /// </summary>        
        /// <param name="from">O remetente</param>
        /// <param name="to">O destinatário</param>
        /// <param name="subject">O assunto</param>
        /// <param name="message">A mensagem</param>        
        /// <returns>True em caso de sucesso</returns>
        public static Boolean SendEmail(String from, String to, String subject, String message)
        {
            return SendEmail(defaultConfiguration, from, to, subject, message, null);
        }
        #endregion

    }
}