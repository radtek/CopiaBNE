//-- Data: 10/04/2013 18:52
//-- Autor: Gieyson Stelmak

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace BNE.BLL
{
	public partial class ParametroCurriculo // Tabela: TAB_Parametro_Curriculo
	{

        #region Consultas

        #region Spselectparametrocurriculo
        private const string Spselectparametrocurriculo = @"
        SELECT  PAR.*
        FROM    TAB_Parametro_Curriculo PAR WITH(NOLOCK)
        WHERE   PAR.Idf_Parametro = @idf_Parametro
                AND PAR.Idf_Curriculo = @Idf_Curriculo";
        #endregion

        #endregion

        #region M�todos

        #region CarregarParametroPorCurriculo
        /// <summary>
        /// Carrega um objeto da classe ParametroCurriculo atr�ves do parametro e da filial
        /// </summary>
        /// <returns>objParametroCurriculo</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static bool CarregarParametroPorCurriculo(Enumeradores.Parametro parametro, Curriculo objCurriculo, out ParametroCurriculo objParametroCurriculo, SqlTransaction trans)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Parametro", SqlDbType = SqlDbType.Int, Size = 4, Value = (int)parametro },
                    new SqlParameter { ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = objCurriculo.IdCurriculo }
                };

            IDataReader dr = null;
            if (trans == null)
                {
                    dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spselectparametrocurriculo, parms);
                }
                else
                {
                    dr = DataAccessLayer.ExecuteReader(trans, CommandType.Text, Spselectparametrocurriculo, parms);
                }

            try
            {
               
                objParametroCurriculo = new ParametroCurriculo();
                if (SetInstance(dr, objParametroCurriculo))
                    return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dr.Close();
                dr.Dispose();
            }

            objParametroCurriculo = null;
            return false;
        }
        #endregion

        #endregion

	}
}