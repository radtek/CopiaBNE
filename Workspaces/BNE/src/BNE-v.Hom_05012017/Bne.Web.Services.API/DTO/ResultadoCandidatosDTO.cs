using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bne.Web.Services.API.DTO
{
    /// <summary>
    /// Represenda o resultado da busca de candidatos inscritos em uma vaga.
    /// </summary>
    public class ResultadoCandidatosDTO
    {
        /// <summary>
        /// Total de candidatos inscritos na vaga.
        /// </summary>
        public int TotalRegistros {get; set;}

        /// <summary>
        /// Total de páginas geradas na busca.
        /// </summary>
        public int TotalPaginas { get; set; }

        /// <summary>
        /// Página atual.
        /// </summary>
        public int Pagina { get; set; }

        /// <summary>
        /// Lista de currículos na página atual.
        /// </summary>
        /// <seealso cref="Bne.Web.Services.API.DTO.CurriculoCurtoDTO"/>
        public List<CurriculoCurtoDTO> Curriculos { get; set; }
    }
}