using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BNE.PessoaFisica.Domain.Command
{
    public class Indicacao
    {
        public int IdVaga { get; set; }
        public int? IdCurriculo { get; set; }
        public string CPF { get; set; }
        public IList<AmigoIndicado> listaAmigos { get; set; }
    }
}
