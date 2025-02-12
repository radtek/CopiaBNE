// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace BNE.PessoaFisica.API.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class VagaPerguntaResponse
    {
        /// <summary>
        /// Initializes a new instance of the VagaPerguntaResponse class.
        /// </summary>
        public VagaPerguntaResponse()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the VagaPerguntaResponse class.
        /// </summary>
        public VagaPerguntaResponse(int? idVagaPergunta = default(int?), string descricaoVagaPergunta = default(string), bool? flagResposta = default(bool?), int? tipoResposta = default(int?))
        {
            IdVagaPergunta = idVagaPergunta;
            DescricaoVagaPergunta = descricaoVagaPergunta;
            FlagResposta = flagResposta;
            TipoResposta = tipoResposta;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "IdVagaPergunta")]
        public int? IdVagaPergunta { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "DescricaoVagaPergunta")]
        public string DescricaoVagaPergunta { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "FlagResposta")]
        public bool? FlagResposta { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "TipoResposta")]
        public int? TipoResposta { get; set; }

    }
}
