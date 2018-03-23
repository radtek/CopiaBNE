//-- Data: 02/03/2010 09:17
//-- Autor: Gieyson Stelmak

using System;

namespace BNE.BLL
{
	public partial class Regiao // Tabela: TAB_Regiao
	{
        #region [ Migration Mapping ]
        /// <summary>
        /// M�dodos e atributos auxiliares � migra��o de dados para o novo
        /// dom�nio.
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