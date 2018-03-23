using System;

namespace BNE.PessoaFisica.Domain.Model
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
