using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bne.Web.Services.API.DTO.Integracao
{
    /// <summary>
    /// Classe que contém as informações de funções pretendidas.
    /// </summary>
    public class ExportaCandidatoFuncoesParam
    {
        /// <summary>
        /// Nome da função pretendida pelo candidato.
        /// </summary>
        public string Funcao { get; set; }

        /// <summary>
        /// Quantidade de meses que o candidato atuou na função.
        /// </summary>
        public short? Experiencia { get; set; }
    }
}