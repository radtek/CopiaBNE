using System;

namespace BNE.PessoaJuridica.Domain.Command
{
    public class CriarSolicitacaoReceita
    {
        public string CNPJ { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public DateTime DataCadastro { get; set; }
    }
}
