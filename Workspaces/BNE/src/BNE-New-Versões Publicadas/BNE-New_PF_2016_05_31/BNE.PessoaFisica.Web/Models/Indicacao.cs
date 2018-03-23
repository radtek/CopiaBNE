using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BNE.PessoaFisica.Web.Models
{
    public class Indicacao
    {
        public int IdVaga { get; set; }
        public int? IdCurriculo { get; set; }
        public string CPF { get; set; }
        public IList<AmigoIndicado> listaAmigos { get; set; }
    }
}