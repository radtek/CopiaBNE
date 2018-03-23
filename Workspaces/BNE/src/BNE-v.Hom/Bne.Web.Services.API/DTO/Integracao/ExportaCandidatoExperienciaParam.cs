using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bne.Web.Services.API.DTO.Integracao
{
    public class ExportaCandidatoExperienciaParam
    {
        /// <summary>
        /// Nome da empresa
        /// </summary>
        [Required]
        public string Empresa { get; set; }

        /// <summary>
        /// <br/>
        /// AreaBNE
        /// </summary>
        [Required]
        public int AreaBNE { get; set; }

        /// <summary>
        /// <br/>
        /// Data de admisssão
        /// </summary>
        public DateTime? DataAdmissao { get; set; }

        /// <summary>
        /// <br/>
        /// Data de demissão
        /// </summary>
        public DateTime? DataDemissao { get; set; }


        /// <summary>
        /// <br/>
        /// Data de admisssão
        /// </summary>
        [Required]
        public string Funcao { get; set; }

    }
}