using Bne.Web.Services.API.DTO.Enum;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Bne.Web.Services.API.DTO.CadastroCV
{
    /// <summary>
    /// Classe com as informações do idioma do candidato
    /// </summary>
    public class CadastroIdioma
    {
        /// <summary>
        /// Descrição do idioma.
        /// Deve ser um dos valores indicados na tabela Idiomas
        /// </summary>
        [Required]
        [DataMember]
        public string DescricaoIdioma { get; set; }
        
        /// <summary>
        /// Nível do idioma
        /// </summary>
        [Required]
        [DataMember]
        public NivelIdioma NivelIdioma { get; set; }
    }
}