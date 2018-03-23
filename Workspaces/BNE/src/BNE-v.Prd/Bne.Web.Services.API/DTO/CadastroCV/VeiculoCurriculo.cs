using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Bne.Web.Services.API.DTO.CadastroCV
{
    /// <summary>
    /// Classe com informações sobre veículos
    /// </summary>
    public class VeiculoCurriculo
    {
        /// <summary>
        /// Tipo do veículo.
        /// Deve conter um dos valores presentes na tabela TiposVeiculos
        /// </summary>
        [Required]
        [DataMember]
        public string TipoVeiculo { get; set; }

        /// <summary>
        /// Descrição do modelo do veículo
        /// </summary>
        [DataMember]
        public string DescricaoModelo { get; set; }

        /// <summary>
        /// Ano de fabricação do veículo
        /// </summary>
        [DataMember]
        public short Ano { get; set; }
    }
}