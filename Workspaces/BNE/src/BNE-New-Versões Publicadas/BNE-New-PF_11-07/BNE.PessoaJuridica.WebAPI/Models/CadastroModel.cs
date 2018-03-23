using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BNE.PessoaJuridica.WebAPI.Models
{
    public class CadastroModel
    {

        [Required(ErrorMessage = "O campo NumeroCNPJ é obrigatório.")]
        public string NumeroCNPJ { get; set; }
        public string RazaoSocial { get; set; }
        public string Site { get; set; }
        [EmailAddress(ErrorMessage = "Email inválido.")]
        public string Email { get; set; }
        public string NomeFantasia { get; set; }
        [Required(ErrorMessage = "O campo NumeroComercial é obrigatório.")]
        public string NumeroComercial { get; set; }
        public bool Matriz { get; set; }
        public string SituacaoCadastral { get; set; }
        public string CNAE { get; set; }
        public string NaturezaJuridica { get; set; }
        [StringLength(8, MinimumLength = 8, ErrorMessage = "O campo CEP deve ter 8 dígitos.")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "O campo CEP deve conter apenas números.")]
        public string CEP { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Complemento { get; set; }
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string IP { get; set; }
        [Required(ErrorMessage = "O campo QuantidadeFuncionario é obrigatório.")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "O campo QuantidadeFuncionario deve conter apenas números.")]
        public int QuantidadeFuncionario { get; set; }

        public DateTime? DataAbertura { get; set; }
        [Required(ErrorMessage = "Usuario é obrigatório.")]
        public CadastroUsuarioModel Usuario { get; set; }
        public List<CadastroUsuarioAdicionalModel> UsuariosAdicionais { get; set; }

    }

    public class CadastroUsuarioAdicionalModel
    {

        public string Nome { get; set; }
        [EmailAddress(ErrorMessage = "Email do usuário adicional inválido.")]
        public string Email { get; set; }

    }
}