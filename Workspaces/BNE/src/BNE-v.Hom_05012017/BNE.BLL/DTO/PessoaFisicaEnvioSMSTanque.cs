using System;

namespace BNE.BLL.DTO
{
    public class PessoaFisicaEnvioSMSTanque
    {
        public int idDestinatario { get; set; }
        public string dddCelular { get; set; }
        public string numeroCelular { get; set; }
        public string nomePessoa { get; set; }
        public string emailPessoa { get; set; }
        public string mensagem { get; set; }
        public decimal numeroCPF { get; set; }
        public DateTime dataNascimento { get; set; }
    }
}
