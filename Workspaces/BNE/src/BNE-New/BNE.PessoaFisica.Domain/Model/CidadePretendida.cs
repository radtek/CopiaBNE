using System;

namespace BNE.PessoaFisica.Domain.Model
{
    public class CidadePretendida
    {
        public Int64 Id { get; set; }
        public DateTime DataCadastro { get; set; }

        public virtual PessoaFisica PessoaFisica { get; set; }
        public virtual Global.Model.Cidade CidadeGlobal { get; set; }
    }
}