using System;

namespace BNE.PessoaFisica.Domain.Model
{
    public class Foto
    {
        public Int64 Id { get; set; }
        public string Url { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataAtualizacao { get; set; }

        public virtual Model.PessoaFisica PessoaFisica { get; set; }
    }
}