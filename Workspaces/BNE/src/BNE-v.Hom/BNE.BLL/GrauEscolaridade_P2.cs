//-- Data: 02/03/2010 09:20
//-- Autor: Gieyson Stelmak

using System;
using System.Data;
namespace BNE.BLL
{
	public partial class GrauEscolaridade // Tabela: TAB_Grau_Escolaridade
	{
        #region Consultas
        private const string SELECTGRAU = @" SELECT	Idf_Grau_Escolaridade, Des_Grau_Escolaridade 
                                                FROM	plataforma.TAB_Grau_Escolaridade 
                                                WHERE	Flg_Inativo = 0
                                                ORDER BY Idf_Grau_Escolaridade ASC";

        #endregion

        #region M�todos

        #region Listar
        /// <summary>
        /// M�todo retorna Consulta para carregar DropDownList
        /// </summary>
        /// <returns></returns>
        public static IDataReader Listar()
        {
            return DataAccessLayer.ExecuteReader(CommandType.Text, SELECTGRAU, null);
        }
        #endregion

        #endregion

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