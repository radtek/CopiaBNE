using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bne.Web.Services.API.DTO.Integracao
{
    public class AtualizaUsuario
    {
        [Required(ErrorMessage = "É necessário informar um nome para o candidato.")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "Informe a data de nascimento.")]
        [DataType(DataType.Date)]
        public DateTime DataNascimento { get; set; }
        [Required(AllowEmptyStrings = true)]
        [RegularExpression(@"(^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,4}(?:\.[a-z]{2})?)$)", ErrorMessage = "Endereço de email inválido")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Função Obrigátoria")]
        public List<ExportaCandidatoFuncoesParam> Funcoes { get; set; }
        [Required(ErrorMessage = "Cidade Obrigátoria")]
        public string Cidade { get; set; }
        [Required(ErrorMessage = "Pretensão salarial não informada.")]
        public decimal Salario { get; set; }
        [Required(ErrorMessage = "Número de DDD é obrigatório.")]
        [RegularExpression(@"^[1-9][0-9]", ErrorMessage = "Número de DDD inválido")]
        [MaxLength(2, ErrorMessage = "O Número de DDD deve ter no máximo 2 dígitos.")]
        public string DDDCelular { get; set; }
        [Required(ErrorMessage = "O telefone celular é obrigatório")]
        [RegularExpression(@"(^[9][0-9][0-9]{7})|(^[6-9][0-9]{7})", ErrorMessage = "Número de celular inválido")]
        [MaxLength(9, ErrorMessage = "O número de celular não pode exceder 9 dígitos.")]
        public string Celular { get; set; }
        [Required]
        public int Escolaridade { get; set; }
        [Required(ErrorMessage = "O número de CPF é obrigatório.")]
        public decimal CPF { get; set; }
        public int? Sexo { get; set; }
    }
}