using System;
using BNE.Core.Common;

namespace BNE.PessoaFisica.Domain.Model
{
    public class CodigoConfirmacaoEmail
    {
        public int Id { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataUtilizacao { get; set; }
        public string Email { get; set; }
        public string Codigo { get; set; }

        internal CodigoConfirmacaoEmail() { } //EF

        public CodigoConfirmacaoEmail(string email)
        {
            string token = Utils.ToBase64(Guid.NewGuid().ToString());
            Codigo = token;
            DataCriacao = DateTime.Now;
            Email = email;
        }

        /// <summary>
        /// Utiliza o código
        /// </summary>
        public void Utilizar()
        {
            DataUtilizacao = DateTime.Now;
        }
    }
}