using System;
namespace BNE.PessoaFisica.Model
{
    public class Email
    {
        public int IdPessoaFisica { get; set; }
        public string Endereco { get; set; }
        public DateTime DataCadastro { get; set; }

        public virtual PessoaFisica PessoaFisica { get; set; }
    }
}