using System;

namespace BNE.PessoaFisica.Model
{
    public class Foto
    {
        public Int64 Id { get; set; }
        public string Url { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataAtualizacao { get; set; }

        public virtual PessoaFisica PessoaFisica { get; set; }
    }
}