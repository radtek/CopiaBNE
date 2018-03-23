using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bne.Web.Services.API.DTO.Enum
{
    /// <summary>
    /// Enumerador com os possíveis status da vaga.
    /// </summary>
    public enum StatusVaga
    {
        /// <summary>
        /// Vaga aguardando auditoria
        /// </summary>
        EmPublicacao,
        /// <summary>
        /// Vaga está ativa e sendo exibida para os candidatos
        /// </summary>
        Ativa,
        /// <summary>
        /// Vaga está sendo exibida aos candidatos com a informação de que o processo seletivo foi encerrado, mas a empresa aceita currículos
        /// </summary>
        Arquivada,
        /// <summary>
        /// Vaga não está sendo exibida
        /// </summary>
        Inativa
    }
}