// Code generated by Microsoft (R) AutoRest Code Generator 0.17.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace BNE.WebServices.API.Client.Models
{
    using System.Linq;

    /// <summary>
    /// Classe que contém informações adicinais para a candidatura
    /// </summary>
    public partial class Candidatura
    {
        /// <summary>
        /// Initializes a new instance of the Candidatura class.
        /// </summary>
        public Candidatura() { }

        /// <summary>
        /// Initializes a new instance of the Candidatura class.
        /// </summary>
        /// <param name="respostas">Respostas fornecidas pelo candidato</param>
        public Candidatura(System.Collections.Generic.IList<RespostaPergunta> respostas = default(System.Collections.Generic.IList<RespostaPergunta>))
        {
            Respostas = respostas;
        }

        /// <summary>
        /// Gets or sets respostas fornecidas pelo candidato
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "Respostas")]
        public System.Collections.Generic.IList<RespostaPergunta> Respostas { get; set; }

    }
}
