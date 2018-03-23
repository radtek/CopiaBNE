using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BNE.Core.Models
{
    /// <summary>
    /// Model padrão para o retorno de resultados paginados.
    /// </summary>
    /// <typeparam name="TObject">Necessita ser uma classe</typeparam>
    public class PaginatedResult<TObject> where TObject : class
    {
        /// <summary>
        /// Cria o resultado para uma busca paginada.
        /// </summary>
        /// <param name="page">Página que está sendo retornada</param>
        /// <param name="recordsPerPage">Número de Registros por Página</param>
        /// <param name="totalOfRecords">Número total de registros da busca</param>
        /// <param name="results">Lista com os resultados da página</param>
        public PaginatedResult(int page, int recordsPerPage, int totalOfRecords, List<TObject> results)
        {
            Page = page; RecordsPerPage = recordsPerPage; TotalOfRecords = totalOfRecords; Results = results;
        }

        /// <summary>
        /// Página do resultados
        /// </summary>
        public int Page;

        /// <summary>
        /// Número de registros por página
        /// </summary>
        public int RecordsPerPage;

        /// <summary>
        /// Número de total de registros da busca
        /// </summary>
        public int TotalOfRecords;

        /// <summary>
        /// Número de total de registros da busca
        /// </summary>
        public int TotalOfPages { get { return (int)Math.Ceiling((decimal)TotalOfRecords / (decimal)RecordsPerPage); } }

        public List<TObject> Results;
    }
}
