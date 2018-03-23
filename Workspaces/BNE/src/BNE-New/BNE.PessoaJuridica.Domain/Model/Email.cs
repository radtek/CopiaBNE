using System;

namespace BNE.PessoaJuridica.Domain.Model
{
    public class Email
    {

        public Int64 Id { get; set; }
        public string Endereco { get; set; }
        public DateTime DataCadastro { get; set; }

        public virtual PessoaJuridica PessoaJuridica { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual UsuarioPessoaJuridica UsuarioPessoaJuridica { get; set; }

    }
}
