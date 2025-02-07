// Code generated by Microsoft (R) AutoRest Code Generator 0.17.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace BNE.WebServices.API.Client.Models
{
    using System.Linq;

    /// <summary>
    /// Represenda o resultado da busca de candidatos inscritos em uma vaga.
    /// </summary>
    public partial class ResultadoCandidatosDTOCurriculo
    {
        /// <summary>
        /// Initializes a new instance of the ResultadoCandidatosDTOCurriculo
        /// class.
        /// </summary>
        public ResultadoCandidatosDTOCurriculo() { }

        /// <summary>
        /// Initializes a new instance of the ResultadoCandidatosDTOCurriculo
        /// class.
        /// </summary>
        /// <param name="totalRegistros">Total de candidatos inscritos na
        /// vaga.</param>
        /// <param name="totalPaginas">Total de páginas geradas na
        /// busca.</param>
        /// <param name="pagina">Página atual.</param>
        /// <param name="curriculos">Lista de currículos na página
        /// atual.</param>
        public ResultadoCandidatosDTOCurriculo(int? totalRegistros = default(int?), int? totalPaginas = default(int?), int? pagina = default(int?), System.Collections.Generic.IList<Curriculo> curriculos = default(System.Collections.Generic.IList<Curriculo>))
        {
            TotalRegistros = totalRegistros;
            TotalPaginas = totalPaginas;
            Pagina = pagina;
            Curriculos = curriculos;
        }

        /// <summary>
        /// Gets or sets total de candidatos inscritos na vaga.
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "TotalRegistros")]
        public int? TotalRegistros { get; set; }

        /// <summary>
        /// Gets or sets total de páginas geradas na busca.
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "TotalPaginas")]
        public int? TotalPaginas { get; set; }

        /// <summary>
        /// Gets or sets página atual.
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "Pagina")]
        public int? Pagina { get; set; }

        /// <summary>
        /// Gets or sets lista de currículos na página atual.
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "Curriculos")]
        public System.Collections.Generic.IList<Curriculo> Curriculos { get; set; }

    }
}
