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

        #region Métodos

        #region Listar
        /// <summary>
        /// Método retorna Consulta para carregar DropDownList
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