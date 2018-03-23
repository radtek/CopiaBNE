//-- Data: 15/05/2013 15:50
//-- Autor: Gieyson Stelmak

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace BNE.BLL
{
	public partial class RegraSubstituicaoIntegracao // Tabela: TAB_Regra_Substituicao_Integracao
    {
        #region Propriedades

        #region CampoIntegrador
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public Enumeradores.CampoIntegrador? CampoIntegrador
        {
            get
            {
                if (this._campoIntegrador == null)
                {
                    return null;
                }
                return (Enumeradores.CampoIntegrador)this._campoIntegrador.IdCampoIntegrador;
            }
            set
            {
                if (this._campoIntegrador == null)
                {
                    this._campoIntegrador = new CampoIntegrador();
                }
                this._campoIntegrador.IdCampoIntegrador = (int)value;
                this._modified = true;
            }
        }
        #endregion

        #endregion
    }
}