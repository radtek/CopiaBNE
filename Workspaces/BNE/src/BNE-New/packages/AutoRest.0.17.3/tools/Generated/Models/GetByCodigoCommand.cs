// Code generated by Microsoft (R) AutoRest Code Generator 0.17.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace .Models
{
    using System.Linq;

    public partial class GetByCodigoCommand
    {
        /// <summary>
        /// Initializes a new instance of the GetByCodigoCommand class.
        /// </summary>
        public GetByCodigoCommand() { }

        /// <summary>
        /// Initializes a new instance of the GetByCodigoCommand class.
        /// </summary>
        public GetByCodigoCommand(string codigo = default(string))
        {
            Codigo = codigo;
        }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "Codigo")]
        public string Codigo { get; set; }

    }
}
