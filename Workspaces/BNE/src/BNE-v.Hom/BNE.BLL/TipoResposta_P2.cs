//-- Data: 31/05/2011 17:27
//-- Autor: Vinicius Maciel

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace BNE.BLL
{
	public partial class TipoResposta // Tabela: BNE_Tipo_Resposta
	{
        #region [ Migration Mapping ]
        /// <summary>
        /// Médodos e atributos auxiliares à migração de dados para o novo
        /// domínio.
        /// </summary>
        public int MigrationId
        {
            set
            {
                this._idTipoResposta = value;
            }
            get { return this._idTipoResposta; }
        }
        #endregion
    }


}