using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Employer.Componentes.UI.Web.Interface
{
    public interface IMensagemErro
    {
        String MensagemErroObrigatorioSummary { get; set; }
        String MensagemErroFormatoSummary { get; set; }
        String MensagemErroInvalidoSummary { get; set; }
    }
}
