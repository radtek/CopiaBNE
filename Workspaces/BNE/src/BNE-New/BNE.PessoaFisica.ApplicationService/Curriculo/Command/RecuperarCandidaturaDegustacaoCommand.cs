using System;

namespace BNE.PessoaFisica.ApplicationService.Curriculo.Command
{
    public class RecuperarCandidaturaDegustacaoCommand
    {
        public Decimal CPF { get; set; }
        public DateTime Data { get; set; }
    }
}