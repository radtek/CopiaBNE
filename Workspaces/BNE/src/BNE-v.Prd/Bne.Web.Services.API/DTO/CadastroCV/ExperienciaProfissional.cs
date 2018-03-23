using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Bne.Web.Services.API.DTO.CadastroCV
{
    /// <summary>
    /// Classe com as informações de experiências profissionais
    /// </summary>
    [DataContract]
    public class CadastroExperienciaProfissional
    {
        /// <summary>
        /// Nome da empresa da experiência
        /// </summary>
        [Required]
        [DataMember]
        public string NomeEmpresa { get; set; }

        /// <summary>
        /// Área de atuação da empresa.
        /// Valor informado deve estar presente na tabela Areas
        /// </summary>
        [Required]
        [DataMember]
        public string Area { get; set; }

        /// <summary>
        /// Data de entrada na empresa
        /// </summary>
        [Required]
        [DataMember]
        public DateTime DataAdmissao { get; set; }

        /// <summary>
        /// Data de saída da empresa
        /// </summary>
        [DataMember]
        public DateTime? DataDemissao { get; set; }

        /// <summary>
        /// Função exercida na experiência
        /// </summary>
        [Required]
        [DataMember]
        public string Funcao { get; set; }

        /// <summary>
        /// Descrição das atividades exercidas
        /// </summary>
        [Required]
        [DataMember]
        public string Atribuicoes { get; set; }

        /// <summary>
        /// Valor do último salário recebido
        /// </summary>
        [Required]
        [DataMember]
        public decimal Salario { get; set; }
    }
}