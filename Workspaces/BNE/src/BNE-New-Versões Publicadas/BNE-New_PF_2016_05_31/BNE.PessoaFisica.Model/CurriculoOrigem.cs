using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BNE.PessoaFisica.Model
{
    public class CurriculoOrigem
    {
        public int Id { get; set; }
        public DateTime DataCadastro { get; set; }

        public virtual Curriculo Curriculo { get; set; }
        public virtual Global.Model.OrigemGlobal OrigemGlobal { get; set; }
    }
}