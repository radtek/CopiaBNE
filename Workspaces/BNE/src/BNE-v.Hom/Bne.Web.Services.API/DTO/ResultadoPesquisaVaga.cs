using Bne.Web.Services.API.DTO.Util;
using System.Collections.Generic;

namespace Bne.Web.Services.API.DTO
{
    /// <summary>
    /// Classe para o retorno de pesquisa de vagas
    /// </summary>
    public class ResultadoPesquisaVaga : ResultadoPaginado<DTO.Vaga>
    {
        /// <summary>
        /// Lista com a sumarização dos campos solicitados
        /// </summary>
        public List<FacetField> Facets { get; set; }

        /// <summary>
        /// Construtor para uma instancia de resultados paginados
        /// </summary>
        /// <param name="totalRegistros">Total de registros presentes na pesquisa</param>
        /// <param name="pagina">Página atual da pesquisa</param>
        /// <param name="registrosPorPagina">Número de registros por página</param>
        /// <param name="registros">List com os registros retornados</param>
        public ResultadoPesquisaVaga(int totalRegistros,
            int pagina,
            int registrosPorPagina,
            IEnumerable<DTO.Vaga> registros)
            :base(totalRegistros, pagina, registrosPorPagina, registros)
        {
            Facets = new List<FacetField>();
        }

    }
}