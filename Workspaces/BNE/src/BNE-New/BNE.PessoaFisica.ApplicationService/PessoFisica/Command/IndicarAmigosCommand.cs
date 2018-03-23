using System;
using System.Collections.Generic;

namespace BNE.PessoaFisica.ApplicationService.PessoFisica.Command
{
    public class IndicarAmigosCommand
    {
        public int IdVaga { get; set; }
        public int? IdCurriculo { get; set; }
        public string CPF { get; set; }
        public DateTime DataNascimento { get; set; }
        public IList<IndicarAmigosAmigoIndicadoCommand> ListaAmigos { get; set; }
    }
}