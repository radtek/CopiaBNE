using System;
using System.Collections.Generic;
using BNE.Mensagem.Core;
using BNE.Services.AsyncServices.Base;
using BNE.Services.AsyncServices.Base.Plugins;

namespace BNE.Mensagem.AsyncExecutor
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
            set
            {
                base.TimeoutMail = value;
            }
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
            set
            {
                base.TimeoutSMS = value;
            }
        }

        public override bool SendMail(string from, string to, string subject, string message, Dictionary<string, byte[]> atachments)
        {
            throw new NotImplementedException();
        }

        public override bool SendMail(string from, string to, string subject, string message)
        {
            try
            {
                if (TimeoutMail.HasValue && TimeoutMail.Value.TotalMilliseconds > 0)
                {
                    var timeout = Math.Min(TimeoutMail.Value.TotalMilliseconds, Int32.MaxValue);
                    SendGridController.Send(from, from,to, to,subject,message,null,null, (int)timeout);
                }
                else
                    SendGridController.Send(from, from, to, to, subject, message);

                return true;
            }
            catch (Exception ex)
            {
                new Services.AsyncServices.Base.Autofac().Logger.Error(ex);
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
                new Services.AsyncServices.Base.Autofac().Logger.Error(ex);
            }

            return false;
        }

        public override void LogError(Exception ex)
        {
            try
            {
                var exception = ex as PluginException;
                if (exception != null)
                    new Services.AsyncServices.Base.Autofac().Logger.Error(ex, exception.PluginName);
                else
                    new Services.AsyncServices.Base.Autofac().Logger.Error(ex);
            }
            catch (Exception ex2)
            {
                new Services.AsyncServices.Base.Autofac().Logger.Error(ex2);
            }
        }

        public override void LogError(Exception ex, PluginBase objPlugin)
        {
            try
            {
                new Services.AsyncServices.Base.Autofac().Logger.Error(ex, objPlugin.MetadataName);
            }
            catch (Exception ex2)
            {
                new Services.AsyncServices.Base.Autofac().Logger.Error(ex2);
            }
        }

        public override void LogError(Exception ex, string customMessage)
        {
            try
            {
                new Services.AsyncServices.Base.Autofac().Logger.Error(ex, customMessage);
            }
            catch (Exception ex2)
            {
                new Services.AsyncServices.Base.Autofac().Logger.Error(ex2);
            }
        }

        public override void LogErrorAndSendEmail(Exception ex, String to, PluginBase objPlugin)
        {
            try
            {
                new Services.AsyncServices.Base.Autofac().Logger.Error(ex);

                SendMail("gieyson@bne.com.br", to, "Erro no plugin: " + objPlugin.MetadataName,
                    String.Format("<b> Mensagem: {0}</b></br> Stack Trace: </br> <pre>{1}</pre>", ex.Message, ex.StackTrace));
            }
            catch (Exception ex2)
            {
                new Services.AsyncServices.Base.Autofac().Logger.Error(ex2);
            }
        }

    }
}
