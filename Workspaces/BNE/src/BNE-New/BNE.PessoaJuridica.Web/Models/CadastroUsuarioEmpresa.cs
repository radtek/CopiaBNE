using System;

namespace BNE.PessoaJuridica.Web.Models
{
    public class CadastroUsuarioEmpresa
    {

        public string NumeroCNPJ { get; set; }
        public string Nome { get; set; }
        public string NumeroCPF { get; set; }
        public DateTime? DataNascimento { get; set; }
        public string Funcao { get; set; }
        public string Sexo { get; set; }
        public string NumeroCelular { get; set; }
        public bool FlgWhatsApp { get; set; }
        public string Email { get; set; }
        public string NumeroComercial { get; set; }
        public string NumeroComercialRamal { get; set; }
        public string EmailOriginal { get; set; }
        public string IP { get; set; }

    }
}