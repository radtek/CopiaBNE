using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BNE.Web.Vagas.Models
{
    public class CompartilhamentoEmail
    {
        [Required(ErrorMessage = "E-mail é necessário")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"[_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,4})", ErrorMessage = "E-mail inválido.")]
        public string EmailDestinatario { get; set; }
        public List<string> ListaEmailDestinatario { get; set; }
        public int IdentificadorVaga { get; set; }
        public string URL { get; set; }
        public bool PodeInserirMaisEmails { get; set; }

        public CompartilhamentoEmail()
        {
            ListaEmailDestinatario = new List<string>();
        }
    }
}