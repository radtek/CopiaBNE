using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bne.Web.Services.API.DTO.Integracao
{
    /// <summary>
    /// Candidato a que será exportado para o BNE
    /// </summary>
    public class ExportaCandidatoParam
    {
        /// <summary>
        /// Número de CPF do candidato.
        /// </summary>
        [Required(ErrorMessage = "O número de CPF é obrigatório.")]
        public decimal CPF { get; set; }

        /// <summary>
        /// Data de Nascimento do candidato. Padrão (YYYY-MM-DD)
        /// </summary>
        [Required(ErrorMessage = "Informe a data de nascimento.")]
        [DataType(DataType.Date)]
        public DateTime DataNascimento { get; set; }

        /// <summary>
        /// Número de DDD com dois caracteres.
        /// </summary>
        [Required(ErrorMessage = "Número de DDD é obrigatório.")]
        [RegularExpression(@"^[1-9][0-9]", ErrorMessage = "Número de DDD inválido")]
        [MaxLength(2, ErrorMessage = "O Número de DDD deve ter no máximo 2 dígitos.")]
        public string CelularDDD { get; set; }


        // <summary>
        /// Número de telefone celular com 9 ou 8 dígitos.
        /// </summary>
        [Required(ErrorMessage = "O telefone celular é obrigatório")]
        [RegularExpression(@"(^[9][0-9][0-9]{7})|(^[6-9][0-9]{7})", ErrorMessage = "Número de celular inválido")]
        [MaxLength(9, ErrorMessage = "O número de celular não pode exceder 9 dígitos.")]
        public string Celular { get; set; }


        /// <summary>
        /// Endereço de email do candidato.
        /// </summary>
        [Required(AllowEmptyStrings = true)]
        [RegularExpression(@"(^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,4}(?:\.[a-z]{2})?)$)", ErrorMessage = "Endereço de email inválido")]
        public string Email { get; set; }


        /// <summary>
        /// Nome do candidato com no máximo 100 caracteres.
        /// </summary>
        [Required(ErrorMessage = "É necessário informar um nome para o candidato.")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
        public string Nome { get; set; }

        
        /// <summary>
        /// 1 -> Masculino <br/>2-> Feminino
        /// </summary>
        public int? Sexo { get; set; }


        /// <summary>
        /// Formato "NomeCidade/UF"
        /// </summary>
        [Required(ErrorMessage = "Informe a cidade do candidato.")]
        public string Cidade { get; set; }

        /// <summary>
        /// <br/>
        /// Código da escolaridade definido na tabela de plataforma.
        /// </summary>
        [Required]
        public int Escolaridade { get; set; }

        /// <summary>
        /// Array com as funções pretendidas.
        /// </summary>
        [Required(ErrorMessage = "Informe a lista de funções pretendidas")]
        public List<ExportaCandidatoFuncoesParam> Funcoes  { get; set; }

        /// <summary>
        /// Salário desejado pelo candidato
        /// </summary>
        [Required(ErrorMessage = "Pretensão salarial não informada.")]
        public decimal PretensaoSalarial { get; set; }

        /// <summary>
        /// Origem dos dados do candidato
        /// </summary>
        [Required(ErrorMessage = "Origem do candidato não informada.")]
        public string Origem { get; set; }


        /// <summary>
        /// Url de origem SINE
        /// </summary>
        [Required(ErrorMessage = "URL de origem do SINE.")]
        public string OrigemURL { get; set; }

        /// <summary>
        /// UTMSource
        /// </summary>
        [Required(ErrorMessage = "UTM Source é requerido")]
        public string UTMSource { get; set; }

        /// <summary>
        /// DesPalavraChave
        /// </summary>
        [Required(ErrorMessage = "Palavra chave da requisição")]
        public string DesPalavraChave { get; set; }

        /// <summary>
        /// Bairro
        /// </summary>
        public string Bairro { get; set; }

        public List<ExportaCandidatoExperienciaParam> Experiencias { get; set; }

        public List<ExportaCandidatoFormacoesParam> Formacoes { get; set; }
         
    }
}