using System;

namespace BNE.PessoaJuridica.WebAPI.Models
{
    public class CadastroReceita
    {

        public string CNPJ { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public DateTime DataCadastro { get; set; }

    }
}