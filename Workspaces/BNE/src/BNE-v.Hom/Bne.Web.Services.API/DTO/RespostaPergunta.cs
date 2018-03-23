using System.Runtime.Serialization;

namespace Bne.Web.Services.API.DTO
{
    /// <summary>
    /// Classe que representa uma resposta fornecida pelo candidato
    /// </summary>
    [DataContract]
    public class RespostaPergunta
    {
        /// <summary>
        /// Identificador da pergunta
        /// </summary>
        [DataMember(Name = "IdPergunta")]
        public int IdPergunta { get; set; }

        /// <summary>
        /// Resposta fornecida pelo candidato
        /// </summary>
        [DataMember(Name = "Resposta")]
        public bool Resposta { get; set; }
    }
}