// Code generated by Microsoft (R) AutoRest Code Generator 0.17.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace BNE.WebServices.API.Client.Models
{
    using System.Linq;

    public partial class Idioma
    {
        /// <summary>
        /// Initializes a new instance of the Idioma class.
        /// </summary>
        public Idioma() { }

        /// <summary>
        /// Initializes a new instance of the Idioma class.
        /// </summary>
        public Idioma(string descricaoIdioma = default(string), string nivelIdioma = default(string))
        {
            DescricaoIdioma = descricaoIdioma;
            NivelIdioma = nivelIdioma;
        }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "DescricaoIdioma")]
        public string DescricaoIdioma { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "NivelIdioma")]
        public string NivelIdioma { get; set; }

    }
}
