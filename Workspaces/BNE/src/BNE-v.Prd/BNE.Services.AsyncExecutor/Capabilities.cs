using System;
using System.Collections.Generic;
using System.Linq;
using BNE.BLL.AsyncServices;
using BNE.BLL.Mensagem;
using BNE.EL;
using BNE.Services.AsyncServices;
using BNE.Services.AsyncServices.Plugins;
using MailSender;

namespace BNE.Services.AsyncExecutor
{
    public class Capabilities : CoreCapabilities
    {
        private readonly TimeSpan _defaultTimeout = TimeSpan.FromMinutes(20);
        public static IMailSenderAPI MailSenderApi = new MailSenderAPI();
        private static readonly EmailService _emailService = new EmailService();
        public readonly string ProcessKeyTransacional = Parametro.RecuperaValorParametro(BLL.AsyncServices.Enumeradores.Parametro.MailSenderAPIKey);

        public override TimeSpan? TimeoutMail
        {
            get
            {
                if (base.TimeoutMail.HasValue)
                {
                    return base.TimeoutMail.Value;
                }

                return _defaultTimeout;
            }
            set { base.TimeoutMail = value; }
        }

        public override TimeSpan? TimeoutSMS
        {
            get
            {
                if (base.TimeoutSMS.HasValue)
                {
                    return base.TimeoutSMS.Value;
                }

                return _defaultTimeout;
            }
            set { base.TimeoutSMS = value; }
        }

        public override bool SendMail(string from, string to, string subject, string message, Dictionary<string, byte[]> attachments)
        {
            try
            {
                //MailSenderApi.Mail.Post(new Send(ProcessKeyTransacional, from, new List<string> { to }, subject, message, attachments.ToDictionary(s => s.Key, s => Convert.ToBase64String(s.Value))));
                _emailService.EnviarEmail(ProcessKeyTransacional, from, to, subject, message, attachments.ToDictionary(s => s.Key, s => Convert.ToBase64String(s.Value)), true);
                return true;
            }
            catch (Exception ex)
            {
                GerenciadorException.GravarExcecao(ex);
            }
            return false;
        }

        public override bool SendMail(string from, string to, string subject, string message)
        {
            try
            {
                //MailSenderApi.Mail.Post(new Send(ProcessKeyTransacional, from, new List<string> { to }, subject, message));
                _emailService.EnviarEmail(ProcessKeyTransacional, from, to, subject, message, true);

                return true;
            }
            catch (Exception ex)
            {
                GerenciadorException.GravarExcecao(ex);
            }
            return false;
        }

        public override bool SendSMS(string number, string message, string tracker)
        {
            try
            {
                return SMSController.ObterGateway().Enviar(number, message, tracker);
            }
            catch (Exception ex)
            {
                GerenciadorException.GravarExcecao(ex);
            }

            return false;
        }

        public override void LogError(Exception ex)
        {
            try
            {
                if (ex is PluginException)
                    GerenciadorException.GravarExcecao(ex, (ex as PluginException).PluginName);
                else
                    GerenciadorException.GravarExcecao(ex);
            }
            catch
            {
            }
        }

        public override void LogError(Exception ex, PluginBase objPlugin)
        {
            try
            {
                GerenciadorException.GravarExcecao(ex, objPlugin.MetadataName);
            }
            catch
            {
            }
        }

        public override void LogError(Exception ex, string customMessage)
        {
            try
            {
                GerenciadorException.GravarExcecao(ex, customMessage);
            }
            catch
            {
            }
        }
    }
}