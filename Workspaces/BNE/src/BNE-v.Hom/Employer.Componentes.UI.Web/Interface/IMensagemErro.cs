using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Employer.Componentes.UI.Web.Interface
{
    #pragma warning disable 1591
    public interface IMensagemErro
    {
        /// <summary>
        /// Define a mensagem de erro de campo obrigatório
        /// </summary>
        String MensagemErroObrigatorioSummary { get; set; }

        /// <summary>
        /// Define a mensagem de erro de formato inválido
        /// </summary>
        String MensagemErroFormatoSummary { get; set; }

        /// <summary>
        /// Sem funcionalidade. Obrigado a implementar por causa da Interface.
        /// </summary>
        String MensagemErroInvalidoSummary { get; set; }
    }
}
