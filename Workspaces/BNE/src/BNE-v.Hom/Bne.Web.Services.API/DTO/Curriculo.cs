using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bne.Web.Services.API.DTO
{
    /// <summary>
    /// Classe utilizada para generalizar o retorno do currículo (mini ou completo)
    /// </summary>
    public class Curriculo
    {
        /// <summary>
        /// Data e hora da candidatura. Propriedade somente será preenchida quando retornado pelos endpoints de recuperação de candidatura
        /// </summary>
        public DateTime? DataHoraCandidatura { get; set; }
    }
}