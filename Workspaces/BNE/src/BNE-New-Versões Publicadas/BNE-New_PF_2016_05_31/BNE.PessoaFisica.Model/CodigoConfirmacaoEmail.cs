using System;

namespace BNE.PessoaFisica.Model
{
    public class CodigoConfirmacaoEmail
    {
        public int Id { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataUtilizacao { get; set; }
        public string Email { get; set; }
        public string Codigo { get; set; }
    }
}