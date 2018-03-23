using System;

namespace BNE.PessoaJuridica.Domain.Command
{
    public class CriarOuAtualizarUsuarioEmpresa
    {
        public string Nome { get; set; }
        public char Sexo { get; set; }
        public decimal NumeroCPF { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Funcao { get; set; }
        public string NumeroDDDCelular { get; set; }
        public decimal NumeroCelular { get; set; }
        public string NumeroDDDComercial { get; set; }
        public decimal NumeroComercial { get; set; }
        public decimal NumeroRamal { get; set; }
        public string Email { get; set; }
        public string IP { get; set; }

        public decimal NumeroCNPJ { get; set; }

    }
}
