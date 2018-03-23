using BNE.BLL.AsyncServices;
using BNE.BLL.Custom;
using BNE.Services.AsyncServices;
using BNE.Services.AsyncServices.Plugins;
using BNE.Services.Base.ProcessosAssincronos;
using System;
using System.Collections.Generic;

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
            try
            {
                return BLL.Custom.MailController.Send(to, from, subject, message, atachments);
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
            }
            //return EmailSenderFactory.Create(TipoEnviadorEmail.Smtp).Enviar(subject, message, from, to, atachments.Key, atachments.Value);
            return false;
        }

        public override bool SendMail(string from, string to, string subject, string message)
        {
            try
            {
                if (TimeoutMail.HasValue && TimeoutMail.Value.TotalMilliseconds > 0)
                {
                    var timeout = Math.Min(TimeoutMail.Value.TotalMilliseconds, Int32.MaxValue);
                    MandrillController.Send(to, to, subject, message, from, from, (int)timeout);
                }
                else
                    MandrillController.Send(to, to, subject, message, from, from);
                
                return true;
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
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
                EL.GerenciadorException.GravarExcecao(ex);
            }

            return false;
        }

        public override void LogError(Exception ex)
        {
            try
            {
                if (ex is PluginException)
                    EL.GerenciadorException.GravarExcecao(ex, (ex as PluginException).PluginName);
                else
                    EL.GerenciadorException.GravarExcecao(ex);
            }
            catch { }
        }

        public override void LogError(Exception ex, PluginBase objPlugin)
        {
            try
            {
                EL.GerenciadorException.GravarExcecao(ex, objPlugin.MetadataName);
            }
            catch { }
        }

        public override void LogError(Exception ex, string customMessage)
        {
            try
            {
                EL.GerenciadorException.GravarExcecao(ex, customMessage);
            }
            catch { }
        }

        public override void LogErrorAndSendEmail(Exception ex, String to, PluginBase objPlugin)
        {
            try
            {
                EL.GerenciadorException.GravarExcecao(ex);

                SendMail("gieyson@bne.com.br", to, "Erro no plugin: " + objPlugin.MetadataName,
                    String.Format("<b> Mensagem: {0}</b></br> Stack Trace: </br> <pre>{1}</pre>", ex.Message, ex.StackTrace));
            }
            catch { }
        }

        public override void SaveFile(String filename, byte[] file)
        {
            ProcessoAssincrono.SalvarArquivoGerado(file, filename);
        }

        public override void SetGeneratedFile(int idfAtividade, String filename)
        {
            Atividade.InformarArquivoGerado(idfAtividade, filename);
        }
    }
}
