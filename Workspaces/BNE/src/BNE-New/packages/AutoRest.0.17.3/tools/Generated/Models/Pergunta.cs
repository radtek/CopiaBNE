// Code generated by Microsoft (R) AutoRest Code Generator 0.17.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace .Models
{
    using System.Linq;

    public partial class Pergunta
    {
        /// <summary>
        /// Initializes a new instance of the Pergunta class.
        /// </summary>
        public Pergunta() { }

        /// <summary>
        /// Initializes a new instance of the Pergunta class.
        /// </summary>
        public Pergunta(int? idVagaPergunta = default(int?), string descricaoVagaPergunta = default(string), bool? flagResposta = default(bool?), int? tipoResposta = default(int?), string resposta = default(string), bool? flgRespostaPergunta = default(bool?))
        {
            IdVagaPergunta = idVagaPergunta;
            DescricaoVagaPergunta = descricaoVagaPergunta;
            FlagResposta = flagResposta;
            TipoResposta = tipoResposta;
            Resposta = resposta;
            FlgRespostaPergunta = flgRespostaPergunta;
        }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "IdVagaPergunta")]
        public int? IdVagaPergunta { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "DescricaoVagaPergunta")]
        public string DescricaoVagaPergunta { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "FlagResposta")]
        public bool? FlagResposta { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "TipoResposta")]
        public int? TipoResposta { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "Resposta")]
        public string Resposta { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "FlgRespostaPergunta")]
        public bool? FlgRespostaPergunta { get; set; }

    }
}
