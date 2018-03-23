using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BNE.BLL.Custom.Email
{
    public abstract class EnviadorEmail : ComponenteEnviadorEmail
    {
        #region Propriedades
        protected ComponenteEnviadorEmail Componente { get; set; }
        #endregion

        #region Construtores
        public EnviadorEmail(ComponenteEnviadorEmail componente)
        {
            this.Componente = componente;
        }
        #endregion
    }
}
