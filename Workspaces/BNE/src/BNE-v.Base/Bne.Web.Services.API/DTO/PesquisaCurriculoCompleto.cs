using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bne.Web.Services.API.DTO
{
    public class PesquisaCurriculoCompleto:DTO.Request
    {
        #region IdCurriculo
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public int IdCurriculo
        {
            get;
            set;
        }
        #endregion

        #region FlgDadosdeContato
        /// <summary>
        /// Campo opcional caso não seja colocado valor ele será false.
        /// </summary>
        public bool? FlgDadosdeContato
        {
            get;
            set;
        }
        #endregion
    }
}
