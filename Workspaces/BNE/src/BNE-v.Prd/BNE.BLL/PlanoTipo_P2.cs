//-- Data: 08/04/2010 11:17
//-- Autor: Gieyson Stelmak

using System.Data;
namespace BNE.BLL
{
	public partial class PlanoTipo // Tabela: BNE_Plano_Tipo
	{
        #region Consultas

        private const string SPSELECT = "SELECT * FROM BNE_Plano_Tipo ORDER BY Idf_Plano_Tipo";

        #endregion

        #region Métodos

        #region Listar
        /// <summary>
        /// Método utilizado para retornar todos os registros de Plano_Tipo do banco de dados.
        /// </summary>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Leonardo Saganski</remarks>
        public static IDataReader Listar()
        {
            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECT, null);
        }
        #endregion

        #endregion

	}
}