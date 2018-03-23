//-- Data: 02/03/2010 09:20
//-- Autor: Gieyson Stelmak

using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace BNE.BLL
{
    public partial class Perfil // Tabela: TAB_Perfil
    {

        #region Consultas

        private const string Spselectadministrador = @"
        SELECT  Idf_Perfil, Des_Perfil 
        FROM    TAB_Perfil WITH(NOLOCK) 
        WHERE   Idf_Tipo_Perfil = 3 --Interno
        ORDER BY Des_Perfil";

        #endregion

        #region Métodos

        #region ListarAdministrador
        /// <summary>
        /// Método utilizado para retornar todos os registros da tabela Perfil para a tela de cadastro de usuário Administrador.
        /// </summary>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static IDataReader ListarAdministrador()
        {
            return DataAccessLayer.ExecuteReader(CommandType.Text, Spselectadministrador, null);
        }
        #endregion

        #endregion

    }
}