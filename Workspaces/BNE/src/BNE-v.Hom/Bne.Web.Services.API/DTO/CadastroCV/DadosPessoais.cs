using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Bne.Web.Services.API.DTO.CadastroCV
{
    /// <summary>
    /// Classe com os dados pessoais do candidato
    /// </summary>
    [DataContract]
    public class DadosPessoais
    {
        /// <summary>
        /// Número do RG do candidato
        /// </summary>
        [DataMember]
        public decimal NumeroRg { get; set; }

        /// <summary>
        /// Órgão emissor do RG do candidato
        /// </summary>
        [DataMember]
        public string OrgaoEmissorRg { get; set; }

        /// <summary>
        /// Estado civil do candidato
        /// Um dos valores presentes da tabela EstadosCivis
        /// </summary>
        [Required]
        [DataMember]
        public string EstadoCivil { get; set; }

        /// <summary>
        /// Endereço do candidato
        /// </summary>
        [Required]
        [DataMember]
        public EnderecoCurriculo Endereco { get; set; }

        /// <summary>
        /// DDD do telefone fixo
        /// </summary>
        [DataMember]
        public short DDDTelefoneFixo { get; set; }

        /// <summary>
        /// Número do telefone fixo
        /// </summary>
        [DataMember]
        public decimal NumeroTelefoneFixo { get; set; }

        /// <summary>
        /// DDD do celular para recados do candidato
        /// </summary>
        [DataMember]
        public short DDDCelularRecado { get; set; }

        /// <summary>
        /// Número do celular para recados do candidato
        /// </summary>
        [DataMember]
        public decimal NumeroCelularRecado { get; set; }

        /// <summary>
        /// Pessoa com que deve-se deixar recado no telefone celular de recado
        /// </summary>
        [DataMember]
        public string NomeContatoCelular { get; set; }

        /// <summary>
        /// DDD do telefone fixo para recados do candidato
        /// </summary>
        [DataMember]
        public short DDDTelefoneFixoRecado { get; set; }

        /// <summary>
        /// Número do telefone fixo para recados do candidato
        /// </summary>
        [DataMember]
        public decimal NumeroTelefoneFixoRecado { get; set; }

        /// <summary>
        /// Pessoa com que deve-se deixar recado no telefone fixo de recado
        /// </summary>
        [DataMember]
        public string NomeContatoTelefoneFixo { get; set; }
    }
}