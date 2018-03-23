using System.Collections.Generic;

namespace BNE.Mapper.Models.Indicacao
{
    public class Indicacao
    {
        public int IdVaga { get; set; }
        public int? IdCurriculo { get; set; }
        public string CPF { get; set; }
        public IList<AmigoIndicado> listaAmigos { get; set; }
    }
}
