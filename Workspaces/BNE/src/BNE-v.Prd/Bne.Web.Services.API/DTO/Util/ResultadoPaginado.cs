using System;
using System.Collections.Generic;

namespace Bne.Web.Services.API.DTO.Util
{
    /// <summary>
    /// Classe para retorno de resultados paginados
    /// </summary>
    /// <typeparam name="T">Tipo dos registros retornados</typeparam>
    public class ResultadoPaginado<T> where T : class
    {
        /// <summary>
        /// Total de registros da pesquisa.
        /// </summary>
        public int TotalRegistros { get; private set; }

        /// <summary>
        /// Total de páginas geradas na busca.
        /// </summary>
        public int TotalPaginas { get; private set; }

        /// <summary>
        /// Página atual.
        /// </summary>
        public int Pagina { get; private set; }

        /// <summary>
        /// Número de registros presentes em cada página.
        /// </summary>
        public int RegistrosPorPagina { get; private set; }

        /// <summary>
        /// Lista com os resultados
        /// </summary>
        public IEnumerable<T> Registros;

        /// <summary>
        /// Construtor para uma instancia de resultados paginados
        /// </summary>
        /// <param name="totalRegistros">Total de registros presentes na pesquisa</param>
        /// <param name="pagina">Página atual da pesquisa</param>
        /// <param name="registrosPorPagina">Número de registros por página</param>
        /// <param name="registros">List com os registros retornados</param>
        public ResultadoPaginado(int totalRegistros, 
            int pagina, 
            int registrosPorPagina,
            IEnumerable<T> registros)
        {
            TotalRegistros = totalRegistros;
            Pagina = pagina;
            RegistrosPorPagina = registrosPorPagina;
            TotalPaginas = (int)Math.Ceiling((double)totalRegistros / registrosPorPagina);
            Registros = registros;
        }
    }
}