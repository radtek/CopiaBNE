
using System;

namespace BNE.BLL.Mensagem.DTO
{
    public class DestinatarioSMS
    {
        public int IdDestinatario { get; set; }
        public string DDDCelular { get; set; }
        public string NumeroCelular { get; set; }
        public string NomePessoa { get; set; }
        public string Mensagem { get; set; }
        public Decimal CPF { get; set; }
        public string Email { get; set; }
        public DateTime DataNascimento { get; set; }
    }
}
