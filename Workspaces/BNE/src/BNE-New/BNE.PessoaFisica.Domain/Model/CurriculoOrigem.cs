using System;

namespace BNE.PessoaFisica.Domain.Model
{
    public class CurriculoOrigem
    {
        public int Id { get; set; }
        public DateTime DataCadastro { get; set; }

        public virtual Curriculo Curriculo { get; set; }
        public virtual Global.Model.OrigemGlobal OrigemGlobal { get; set; }
    }
}