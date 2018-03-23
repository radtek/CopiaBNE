using System;
using System.Collections.Generic;
using BNE.BLL.MailService;
using BNE.EL;
using BNE.Services.AsyncServices;
using BNE.Services.AsyncServices.Plugins;

namespace BNE.Services.AsyncExecutor
{
    public class Capabilities : CoreCapabilities
    {
        private readonly TimeSpan _defaultTimeout = TimeSpan.FromMinutes(20);

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

        public override bool SendMail(string from, string to, string subject, string message, Dictionary<string, byte[]> atachments, List<string> tags = null)
        {
            try
            {
                SendgridController.Send(from, to, subject, message, atachments, tags);
                return true;
            }
            catch (Exception ex)
            {
                GerenciadorException.GravarExcecao(ex);
            }
            return false;
        }

        public override bool SendMail(string from, string to, string subject, string message, List<string> tags = null)
        {
            try
            {
                SendgridController.Send(from, to, subject, message, null, tags);
                return true;
            }
            catch (Exception ex)
            {
                GerenciadorException.GravarExcecao(ex);
            }
            return false;
        }

        public override bool SendMailSMTPCloud(string from, string to, string subject, string message)
        {
            try
            {
                SMTPCloudController.Send(from, to, subject, message);
                return true;
            }
            catch (Exception ex)
            {
                GerenciadorException.GravarExcecao(ex);
            }
            return false;
        }

        public override bool SendMailSMTPCloudCampanha(string from, string to, string subject, string message)
        {
            try
            {
                SMTPCloudCampanhaController.Send(from, to, subject, message);
                return true;
            }
            catch (Exception ex)
            {
                GerenciadorException.GravarExcecao(ex);
            }
            return false;
        }

        public override bool SendMailAmazonSES(string from, string to, string assunto, string descricao)
        {
            try
            {
                AmazonSESController.Send(from, to, assunto, descricao);
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