using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Bne.Web.Services.API.DTO.CadastroCV
{
    [DataContract]
    public class EnderecoCurriculo
    {
        /// <summary>
        /// Cep do endereço
        /// </summary>
        [Required]
        [DataMember]
        public decimal Cep { get; set; }

        /// <summary>
        /// Nome da rua, avenida, travessa, etc
        /// </summary>
        [Required]
        [DataMember]
        public string Logradouro { get; set; }

        /// <summary>
        /// Número do endereço.
        /// Aceita string caso o número não esteja disponível.
        /// Recomenda-se o envio de SN para esses casos
        /// </summary>
        [Required]
        [DataMember]
        public string Numero { get; set; }

        /// <summary>
        /// Complemento do endereço
        /// </summary>
        [DataMember]
        public string Complemento { get; set; }

        /// <summary>
        /// Bairro do endereço.
        /// </summary>
        [Required]
        [DataMember]
        public string Bairro { get; set; }

        /// <summary>
        /// Nome da cidade no formato "NomeCidade/SiglaEstado" (Ex.: São Paulo/SP)
        /// </summary>
        [Required]
        [DataMember]
        public string Cidade { get; set; }

    }
}