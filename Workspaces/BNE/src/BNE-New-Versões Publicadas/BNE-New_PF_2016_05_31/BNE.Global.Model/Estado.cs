using System.Collections.Generic;

namespace BNE.Global.Model
{
    public class Estado
    {

        public string UF { get; set; }
        public string Nome { get; set; }

        public virtual ICollection<Cidade> Cidades { get; set; }
    }
}
