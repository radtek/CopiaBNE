using System;
using System.Collections.Generic;

namespace BNE.PessoaJuridica.Web.Models
{
    public class CadastroEmpresa
    {

        public string NumeroCNPJ { get; set; }
        public string Site { get; set; }
        public string NumeroComercial { get; set; }
        public string QuantidadeFuncionario { get; set; }
        public string Captcha { get; set; }
        public string Logradouro { get; set; }
        public string CEP { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string RazaoSocial { get; set; }
        public string NomeFantasia { get; set; }
        public string CNAE { get; set; }
        public string NaturezaJuridica { get; set; }
        public DateTime DataAbertura { get; set; }
        public string SituacaoCadastral { get; set; }
        public string IP { get; set; }
        public bool ReceitaOnline { get; set; }

        public CadastroEmpresaUsuario Usuario { get; set; }
        public List<CadastroEmpresaUsuarioAdicional> UsuariosAdicionais { get; set; }

        public CadastroEmpresa()
        {
            this.Usuario = new CadastroEmpresaUsuario();
            this.UsuariosAdicionais = new List<CadastroEmpresaUsuarioAdicional>();
        }

        public class CadastroEmpresaUsuario
        {
            public string Nome { get; set; }
            public string NumeroCPF { get; set; }
            public DateTime? DataNascimento { get; set; }
            public string Funcao { get; set; }
            public string Sexo { get; set; }
            public string NumeroCelular { get; set; }
            public string Email { get; set; }
            public string NumeroComercial { get; set; }
            public string NumeroComercialRamal { get; set; }
        }
        public class CadastroEmpresaUsuarioAdicional
        {
            public string Nome { get; set; }
            public string Email { get; set; }
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}