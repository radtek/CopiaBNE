using System.Runtime.Serialization;

namespace Bne.Web.Services.API.DTO.CadastroCV
{
    /// <summary>
    /// Classe com as informações de formação
    /// </summary>
    [DataContract]
    public class FormacaoCurriculo
    {
        /// <summary>
        /// Lista com os cursos de formação do candidato.
        /// Nesta lista deve conter os cursos de Ensino Médio, incluindo técnicos,
        /// Graduções e Especializações
        /// </summary>
        [DataMember]
        public Curso[] CursosFormacao { get; set; }

        /// <summary>
        /// Lista com os cursos complementares
        /// </summary>
        [DataMember]
        public CursoComplementar[] CursosComplementares { get; set; }

        /// <summary>
        /// Lista com os idiomas do candidato
        /// </summary>
        [DataMember]
        public CadastroIdioma[] Idiomas;
    }
}