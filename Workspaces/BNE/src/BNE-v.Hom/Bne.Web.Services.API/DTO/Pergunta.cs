using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bne.Web.Services.API.DTO
{
    /// <summary>
    /// Representa uma pergunta feita para candidatos que se inscrevem na vaga.
    /// </summary>
    public class Pergunta
    {
        /// <summary>
        /// Identificador da pergunta.
        /// </summary>
        public int IdPergunta { get; set; }

        /// <summary>
        /// Texto da pergunta.
        /// </summary>
        public string Texto { get; set; }
    }
}