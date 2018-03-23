//-- Data: 02/03/2010 09:17
//-- Autor: Gieyson Stelmak

using System;

namespace BNE.BLL
{
	public partial class Regiao // Tabela: TAB_Regiao
	{
        #region [ Migration Mapping ]
        /// <summary>
        /// Médodos e atributos auxiliares à migração de dados para o novo
        /// domínio.
        /// </summary>
        public DateTime MigrationDataCadastro
        {
            set
            {
                this._dataCadastro = value;
            }
            get { return this._dataCadastro; }
        }
        #endregion
    }
}