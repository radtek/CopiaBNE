using Bne.Web.Services.API.DTO.Enum;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using System.Globalization;

namespace Bne.Web.Services.API.DTO.CadastroCV
{
    /// <summary>
    /// Informações mínimas para um currículo
    /// </summary>
    [DataContract]
    public class CadastroMiniCurriculo
    {
        /// <summary>
        /// Email do candidato
        /// </summary>
        [DataMember]
        public string Email { get; set; }

        /// <summary>
        /// CPF do candidato
        /// </summary>
        [Required]
        [DataMember]
        public decimal Cpf { get; set; }

        /// <summary>
        /// Data dde nascimento do candidato
        /// </summary>
        [Required]
        [DataMember]
        public DateTime DataNascimento { get; set; }

        /// <summary>
        /// DDD do celular do candidato
        /// </summary>
        [Required]
        [DataMember]
        public short DDDCelular { get; set; }

        /// <summary>
        /// Número do celular do candidato
        /// </summary>
        [Required]
        [DataMember]
        public decimal NumeroCelular { get; set; }

        /// <summary>
        /// Nome do candidato
        /// </summary>
        [Required]
        [DataMember]
        public string Nome { get; set; }

        /// <summary>
        /// Sexo do candidato
        /// </summary>
        [Required]
        [DataMember]
        public Sexo Sexo { get; set; }

        /// <summary>
        /// Nome da cidade no formato "NomeCidade/SiglaEstado" (Ex.: São Paulo/SP)
        /// </summary>
        [Required]
        [DataMember]
        public string Cidade { get; set; }

        /// <summary>
        /// Um dos valores presentes na tabela Escolaridades.
        /// Caso o valor não esteja presente nesta tabela, será desconsiderado.
        /// </summary>
        [Required]
        [DataMember]
        public string Escolaridade { get; set; }

        /// <summary>
        /// Flag indicando se o candidato aceita trabalhar como estagiário
        /// Considerados somente para as escolaridades "Ensino Médio Incompleto", 
        /// "Técnico/Pós Médio Incompleto", "Tecnólogo Incompleto" e "Superior Incompleto"
        /// </summary>
        [DataMember]
        public bool AceitoEstagio { get; set; }

        /// <summary>
        /// Funções pretendidas pelo candidato
        /// </summary>
        [DataMember]
        public FuncaoPretendida[] FuncoesPretendidas { get; set; }

        /// <summary>
        /// Pretensão salarial do candidato
        /// </summary>
        [Required]
        [DataMember]
        public decimal PretensaoSalarial { get; set; }
    }
}