using System;
using System.Collections.Generic;

namespace BNE.Mapper.Models.PessoaJuridica
{
    public class PessoaJuridica
    {

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
        public string IP { get; set; }
        public int QuantidadeFuncionario { get; set; }
        public DateTime DataAlteracao { get; set; }
        public DateTime DataAbertura { get; set; }
        public DateTime DataCadastro { get; set; }
        public List<Usuario> Usuarios { get; set; }
        public List<UsuarioAdicional> UsuariosAdicionais { get; set; }

    }
    public class UsuarioAdicional
    {

        public string Nome { get; set; }
        public string Email { get; set; }

    }
}
