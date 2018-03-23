using System.Collections.Generic;

namespace BNE.PessoaFisica.Web.Models
{
    public class Indicacao
    {
        public int IdVaga { get; set; }
        public int? IdCurriculo { get; set; }
        public string CPF { get; set; }
        public IList<AmigoIndicado> ListaAmigos { get; set; }
    }
}