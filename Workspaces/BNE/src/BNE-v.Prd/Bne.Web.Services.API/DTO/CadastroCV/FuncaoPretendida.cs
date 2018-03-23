using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Bne.Web.Services.API.DTO.CadastroCV
{
    /// <summary>
    /// Classe com as informações das funcoes pretendidas pelo candidato
    /// </summary>
    [DataContract]
    public class FuncaoPretendida
    {
        /// <summary>
        /// Função pretendida pelo candidato
        /// O valor informado deve estar presente na tabela Funcoes
        /// </summary>
        [Required]
        [DataMember]
        public string Funcao { get; set; }

        /// <summary>
        /// Meses de experiência do candidato atuando naquela função
        /// </summary>
        [DataMember]
        public short MesesDeExperiencia { get; set; }
    }
}