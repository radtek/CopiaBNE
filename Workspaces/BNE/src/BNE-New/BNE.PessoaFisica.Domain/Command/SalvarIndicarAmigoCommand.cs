using System;
using System.Collections.Generic;

namespace BNE.PessoaFisica.Domain.Command
{
    public class SalvarIndicarAmigoCommand
    {
        public int IdVaga { get; set; }
        public int? IdCurriculo { get; set; }
        public decimal CPF { get; set; }
        public DateTime DataNascimento { get; set; }
        public IList<SalvarIndicarAmigoIndicadoCommand> ListaAmigos { get; set; }
    }
}