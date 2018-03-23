//-- Data: 16/07/2013 16:45
//-- Autor: Gieyson Stelmak

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace BNE.BLL
{
	public partial class MobileToken // Tabela: BNE_Mobile_Token
    {
        #region Consultas

        #region Spcarregar
        private const string Spcarregarporidcurriculotoken = @"
        SELECT  MT.*
        FROM    BNE_Mobile_Token MT WITH(NOLOCK)
        WHERE   MT.Idf_Curriculo = @Idf_Curriculo
        AND     MT.Cod_Dispositivo = @Cod_Dispositivo
        ";
        #endregion

        #endregion

        #region Métodos

        #region CarregarPorIdCurriculoToken
        public static bool CarregarPorIdCurriculoToken(int idCurriculo, string imei, out MobileToken mobileToken)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Cod_Dispositivo", SqlDbType.VarChar, 16));

            parms[0].Value = idCurriculo;
            parms[1].Value = imei;

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spcarregarporidcurriculotoken, parms))
            {
                mobileToken = new MobileToken();
                if (SetInstance(dr, mobileToken))
                {
                    if (!dr.IsClosed)
                        dr.Close();
                    return true;
                }

                if (!dr.IsClosed)
                    dr.Close();

                mobileToken = null;
                return false;
            }
        }
        #endregion

        #endregion
    }
}