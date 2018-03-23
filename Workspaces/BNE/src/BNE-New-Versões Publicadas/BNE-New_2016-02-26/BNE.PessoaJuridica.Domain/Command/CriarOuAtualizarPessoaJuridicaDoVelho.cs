using System;
using System.Collections.Generic;

namespace BNE.PessoaJuridica.Domain.Command
{
    public class CriarOuAtualizarPessoaJuridicaDoVelho
    {
        public int Id { get; set; }
        public decimal NumeroCNPJ { get; set; }
        public string RazaoSocial { get; set; }
        public string NomeFantasia { get; set; }
        public string Site { get; set; }
        public string SituacaoCadastral { get; set; }
        public string CNAE { get; set; }
        public string NaturezaJuridica { get; set; }
        public string Email { get; set; }
        public int CEP { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Complemento { get; set; }
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string NumeroDDDComercial { get; set; }
        public decimal NumeroComercial { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataAbertura { get; set; }
        public DateTime DataAlteracao { get; set; }
        public List<UsuarioPessoaJuridicaDoVelho> Usuarios { get; set; }
    }

    public class UsuarioPessoaJuridicaDoVelho
    {
        public string Nome { get; set; }
        public char Sexo { get; set; }
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
        public DateTime DataAlteracao { get; set; }
        public DateTime DataCadastro { get; set; }
        public string IP { get; set; }
        public bool UsuarioMaster { get; set; }
        public bool UsuarioInativo { get; set; }
    }

}
