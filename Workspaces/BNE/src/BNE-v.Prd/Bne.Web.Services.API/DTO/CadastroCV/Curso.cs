using Bne.Web.Services.API.DTO.Enum;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Bne.Web.Services.API.DTO.CadastroCV
{
    /// <summary>
    /// Informações sobre o curso de formação do candidato
    /// </summary>
    [DataContract]
    public class Curso
    {
        /// <summary>
        /// Nível atual da formação 
        /// Um dos valores presentes na tabela Escolaridades.
        /// </summary>
        [Required]
        [DataMember]
        public string NivelFormacao { get; set; }

        /// <summary>
        /// Nome da instituição onde está cursando ou foi cursado
        /// Obrigatório quando o NivelFormacao igual a 
        /// "Técnico/Pós-Médio Incompleto",
        /// "Técnico/Pós-Médio Completo",
        /// "Tecnólogo Incompleto",
        /// "Superior Incompleto",
        /// "Tecnólogo Completo",
        /// "Superior Completo",
        /// "Pós Graduação / Especialização",
        /// "Mestrado",
        /// "Doutorado"
        /// </summary>
        [DataMember]
        public string Instituicao { get; set; }

        /// <summary>
        /// Nome do curso
        /// Recomenda-se o envio de um dos valores presentes na tabela Cursos.
        /// Obrigatório quando o NivelFormacao igual a 
        /// "Técnico/Pós-Médio Incompleto",
        /// "Técnico/Pós-Médio Completo",
        /// "Tecnólogo Incompleto",
        /// "Superior Incompleto",
        /// "Tecnólogo Completo",
        /// "Superior Completo",
        /// "Pós Graduação / Especialização",
        /// "Mestrado",
        /// "Doutorado"
        /// </summary>
        [DataMember]
        public string NomeCurso { get; set; }

        /// <summary>
        /// Nome da cidade onde foi cursado
        /// Formato: "NomeCidade/SiglaEstado" (Ex.: São Paulo/SP)
        /// Obrigatório quando o NivelFormacao igual a 
        /// "Técnico/Pós-Médio Incompleto",
        /// "Técnico/Pós-Médio Completo",
        /// "Tecnólogo Incompleto",
        /// "Superior Incompleto",
        /// "Tecnólogo Completo",
        /// "Superior Completo",
        /// "Pós Graduação / Especialização",
        /// "Mestrado",
        /// "Doutorado"
        /// </summary>
        [DataMember]
        public string Cidade { get; set; }

        /// <summary>
        /// Ano de conclusão do curso.
        /// Obrigatório quando o NivelFormacao igual a 
        /// "Ensino Médio Completo",
        /// "Técnico/Pós-Médio Completo",
        /// "Tecnólogo Completo",
        /// "Superior Completo",
        /// "Pós Graduação / Especialização",
        /// "Mestrado",
        /// "Doutorado"
        /// </summary>
        [DataMember]
        public short? AnoDeConclusao { get; set; }

        /// <summary>
        /// Atual situação do curso.
        /// Obrigatório quando o NivelFormacao é igual a 
        /// "Ensino Médio Incompleto",
        /// "Técnico/Pós-Médio Incompleto",
        /// "Tecnólogo Incompleto",
        /// "Superior Incompleto"
        /// </summary>
        [DataMember]
        public SituacaoCurso? Situacao { get; set; }

        /// <summary>
        /// Período atual do curso.
        /// Obrigatório quando o NivelFormacao é igual a 
        /// "Tecnólogo Incompleto",
        /// "Superior Incompleto"
        /// </summary>
        [DataMember]
        public short? Periodo { get; set; }
    }
}