using System;

namespace BNE.Mapper.Models.PessoaJuridica
{
    public class Usuario
    {
        public string Nome { get; set; }
        public string Sexo { get; set; }
        public decimal NumeroCPF { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Funcao { get; set; }
        public int? IdFuncaoVelho { get; set; }
        public string NumeroDDDCelular { get; set; }
        public decimal NumeroCelular { get; set; }
        public string NumeroDDDComercial { get; set; }
        public decimal NumeroComercial { get; set; }
        public decimal NumeroRamal { get; set; }
        public string Email { get; set; }
        public string EmailComercial { get; set; }
        public string IP { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataAlteracao { get; set; }
        public bool UsuarioMaster { get; set; }
        public bool UsuarioInativo { get; set; }
    }
}
