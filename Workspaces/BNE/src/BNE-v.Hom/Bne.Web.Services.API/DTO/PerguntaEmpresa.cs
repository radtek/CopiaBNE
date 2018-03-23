using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bne.Web.Services.API.DTO
{
    /// <summary>
    /// Representa uma pergunta feita para candidatos que se inscrevem na vaga.
    /// </summary>
    public class PerguntaEmpresa : Pergunta
    {
        /// <summary>
        /// (Obrigatório) Resposta para questão que pode ser "Sim" ou "Não".
        /// </summary>
        public string Resposta { get; set; }
    }
}