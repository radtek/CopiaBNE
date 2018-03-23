using System;

namespace BNE.Componentes.Interface
{
    public interface IErrorMessage
    {
        String MensagemErroObrigatorioSummary { get; set; }
        String MensagemErroFormatoSummary { get; set; }
        String MensagemErroInvalidoSummary { get; set; }
    }
}
