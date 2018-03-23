//-- Data: 10/07/2010 14:13
//-- Autor: Gieyson Stelmak

using System.Data;

namespace BNE.BLL
{
	public partial class Flag // Tabela: plataforma.TAB_Flag
	{

        #region Consultas
        private const string Spselect = "SELECT * FROM plataforma.TAB_Flag WITH(NOLOCK)";
       
        #endregion

        #region Listar
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Vinicius de Freitas Pereira</remarks>
        public static IDataReader Listar()
        {
            return DataAccessLayer.ExecuteReader(CommandType.Text, Spselect, null);
        }
        #endregion

	}
}