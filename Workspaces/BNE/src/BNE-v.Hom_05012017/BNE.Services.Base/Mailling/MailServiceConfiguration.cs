using System;

namespace BNE.Services.Base.Mailling
{
    /// <summary>
    /// Classe para configuração do serviço de email
    /// </summary>
    public sealed class MailServiceConfiguration
    {

        #region Properties
        /// <summary>
        /// Servidor SMTP
        /// </summary>
        public String SmtpHost { get; set; }
        /// <summary>
        /// Usuário do SMTP
        /// </summary>
        public String SmtpUser { get; set; }
        /// <summary>
        /// Senha do SMTP
        /// </summary>
        public String SmtpPassword { get; set; }
        #endregion

    }
}
