//-- Data: 02/03/2010 09:20
//-- Autor: Gieyson Stelmak

using System.Data;
namespace BNE.BLL
{
	public partial class NivelIdioma // Tabela: TAB_Nivel_Idioma
    {
        #region Consultas

        private const string SELECTNIVELIDIOMA = @" SELECT  
	                                                    Idf_Nivel_Idioma,
	                                                    Des_Nivel_Idioma
                                                    FROM    TAB_Nivel_Idioma
                                                    ORDER BY Idf_Nivel_Idioma";
        #endregion 

        #region Método 
        /// <summary>
        /// Método faz consulta e retorna o DataReader
        /// </summary>
        /// <returns>DataReader</returns>
        public static IDataReader Listar()
        {
            return DataAccessLayer.ExecuteReader(CommandType.Text, SELECTNIVELIDIOMA, null);
        }
        #endregion

    }
}