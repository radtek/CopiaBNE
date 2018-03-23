using System;

namespace BNE.PessoaJuridica.Web.Models
{
    public class CadastroEmpresaReceita
    {

        public string Nome { get; set; }
        public string Email { get; set; }
        public DateTime DataCadastro { get; set; }

        public string CNPJ { get; set; }
        public string Telefone { get; set; }
        

    }
}