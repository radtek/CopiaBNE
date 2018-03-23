using System;
using System.Collections.Generic;
using BNE.Services.AsyncServices.Plugins;

namespace BNE.Services.AsyncServices
{
    /// <summary>
    /// Capacidades que o núcleo do sistema fornece aos plugins
    /// </summary>
    public abstract class CoreCapabilities
    {
        public virtual TimeSpan? TimeoutMail { get; set; }
        public virtual TimeSpan? TimeoutSMS { get; set; }

        public abstract Boolean SendMail(String from, String to, String subject, String message, Dictionary<string, byte[]> atachments, List<string> tags = null);
        public abstract Boolean SendMail(String from, String to, String subject, String message, List<string> tags = null);
        public abstract Boolean SendMailSMTPCloud(String from, String to, String subject, String message);
        public abstract Boolean SendMailSMTPCloudCampanha(string objMensagemFrom, string objMensagemTo, string objMensagemAssunto, string objMensagemDescricao);
        public abstract Boolean SendSMS(string number, string message, string searcher);
        public abstract void LogError(Exception ex);
        public abstract void LogError(Exception ex, PluginBase objPlugin);
        public abstract void LogError(Exception ex, string customMessage);
        public abstract bool SendMailAmazonSES(string from, string to, string assunto, string descricao);
    }
}