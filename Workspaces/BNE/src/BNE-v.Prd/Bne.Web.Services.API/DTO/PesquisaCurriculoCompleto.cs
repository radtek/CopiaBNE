using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Bne.Web.Services.API.DTO
{
    public class PesquisaCurriculoCompleto:DTO.Request
    {
        #region IdCurriculo
        /// <summary>
        /// Id do currículo a ser retornado.
        /// </summary>
        [Required] 
        public int IdCurriculo
        {
            get;
            set;
        }
        #endregion

        #region FlgDadosdeContato
        /// <summary>
        /// Indica se deseja que os dados de contato devem ser retornados.
        /// </summary>
        public bool? FlgDadosdeContato
        {
            get;
            set;
        }
        #endregion
    }
}
