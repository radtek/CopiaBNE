using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BNE.PessoaFisica.Model
{
    public class FuncaoPretendida
    {
        public Int64 Id { get; set; }
        public int? IdFuncao { get; set; }
        public DateTime DataCadastro { get; set; }
        public string Descricao { get; set; }
        public short TempoExperiencia { get; set; }

        public virtual Curriculo Curriculo { get; set; }
    }
}
