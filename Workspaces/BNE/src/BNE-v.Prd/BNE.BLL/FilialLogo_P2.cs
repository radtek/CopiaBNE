//-- Data: 22/05/2014 12:01
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BNE.BLL
{
    public partial class FilialLogo // Tabela: TAB_Filial_Logo
    {

        #region Consultas

        #region Spselectlogo
        private const string Spselectlogo = @"
        SELECT  * 
        FROM    TAB_Filial_Logo
        WHERE   Idf_Filial = @Idf_Filial";
        #endregion Spselectlogo

        #region Sprecuperarlogoporcnpj
        private const string Sprecuperarlogoporcnpj = @"
        SELECT  FL.Img_Logo
        FROM    TAB_Filial_Logo FL WITH(NOLOCK)
                INNER JOIN TAB_Filial F WITH(NOLOCK) ON FL.Idf_Filial = F.Idf_Filial
        WHERE   F.Num_Cnpj = @NumeroCNPJ
                AND FL.Flg_Inativo = 0";
        #endregion

        #endregion

        #region Métodos

        #region CarregarLogo
        /// <summary>
        /// Método responsável por carregar uma instancia de FilialLogo
        /// </summary>
        /// <param name="idFilial">Identificador da Filial</param>
        /// <param name="objFilialLogo">Filial Logo</param>
        /// <returns>True se idFilial tiver valor</returns>
        public static bool CarregarLogo(int idFilial, out FilialLogo objFilialLogo)
        {
            var parms = new List<SqlParameter> { new SqlParameter { ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = idFilial } };

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spselectlogo, parms))
            {
                objFilialLogo = new FilialLogo();
                if (SetInstance(dr, objFilialLogo))
                    return true;

                if (!dr.IsClosed)
                    dr.Close();
            }
            objFilialLogo = null;
            return false;
        }
        #endregion

        #region RecuperarArquivo
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static byte[] RecuperarArquivo(decimal numeroCNPJ)
        {
            var parms = new List<SqlParameter> { new SqlParameter { ParameterName = "@NumeroCNPJ", SqlDbType = SqlDbType.Decimal, Size = 14, Value = numeroCNPJ } };

            object res = DataAccessLayer.ExecuteScalar(CommandType.Text, Sprecuperarlogoporcnpj, parms);

            if (Convert.IsDBNull(res) || res == null || res == DBNull.Value)
                return null;

            return (byte[])res;
        }
        #endregion

        #endregion

    }
}