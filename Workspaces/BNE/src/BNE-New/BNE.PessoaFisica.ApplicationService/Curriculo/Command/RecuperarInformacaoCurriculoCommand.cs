using System;

namespace BNE.PessoaFisica.ApplicationService.Curriculo.Command
{
    public class RecuperarInformacaoCurriculoCommand
    {
        public Decimal CPF { get; set; }
        public int IdVaga { get; set; }
    }
}