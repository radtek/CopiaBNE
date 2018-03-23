using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Bne.Web.Services.API.DTO.CadastroCV
{
    /// <summary>
    /// Informações de cursos complementares do candidato
    /// </summary>
    [DataContract]
    public class CursoComplementar
    {
        /// <summary>
        /// Nome da instituição
        /// </summary>
        [Required]
        [DataMember]
        public string Instituicao { get; set; }

        /// <summary>
        /// Nome do curso
        /// Recomenda-se o envio de um dos valores presentes na tabela Cursos.
        /// </summary>
        [Required]
        [DataMember]
        public string NomeCurso { get; set; }

        /// <summary>
        /// Nome da cidade onde foi cursado
        /// Formato: "NomeCidade/SiglaEstado" (Ex.: São Paulo/SP)
        /// </summary>
        [DataMember]
        public string Cidade { get; set; }

        /// <summary>
        /// Ano de conclusão do curso. Pode ser passado ou futuro.
        /// </summary>
        [DataMember]
        public short? AnoConclusao { get; set; }

        /// <summary>
        /// Carga horária do curso
        /// </summary>
        [DataMember]
        public short? CargaHoraria { get; set; }

    }
}