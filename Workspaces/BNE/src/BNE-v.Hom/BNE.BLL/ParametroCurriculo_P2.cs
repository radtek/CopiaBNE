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

        #region SpListselectparametrocurriculo
        private const string SpListselectparametrocurriculo = @"
        SELECT  PAR.*
        FROM    TAB_Parametro_Curriculo PAR WITH(NOLOCK)
        WHERE   PAR.Idf_Curriculo = @Idf_Curriculo";
        #endregion

        #region Spselectparametrocurriculo
        private const string Spselectparametrocurriculo = @"
        SELECT  PAR.*
        FROM    TAB_Parametro_Curriculo PAR WITH(NOLOCK)
        WHERE   PAR.Idf_Parametro = @idf_Parametro
                AND PAR.Idf_Curriculo = @Idf_Curriculo";
        #endregion

        private const string Spselectvalorparametrocurriculo = @"
        SELECT  PAR.Vlr_Parametro
        FROM    TAB_Parametro_Curriculo PAR WITH(NOLOCK)
        WHERE   PAR.Idf_Parametro = @idf_Parametro
                AND PAR.Idf_Curriculo = @Idf_Curriculo";

        #endregion

        #region Métodos

        #region CarregarParametroPorCurriculo
        /// <summary>
        /// Carrega um objeto da classe ParametroCurriculo atráves do parametro e da filial
        /// </summary>
        /// <returns>objParametroCurriculo</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static List<ParametroCurriculo> CarregarParametroPorCurriculo(int idCurriculo, SqlTransaction trans)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = idCurriculo }
                };

            IDataReader dr = null;
            if (trans == null)
            {
                dr = DataAccessLayer.ExecuteReader(CommandType.Text, SpListselectparametrocurriculo, parms);
            }
            else
            {
                dr = DataAccessLayer.ExecuteReader(trans, CommandType.Text, Spselectparametrocurriculo, parms);
            }

            var lstParametroCurriculo = new List<ParametroCurriculo>();
            try
            {
                var objParametroCurriculo = new ParametroCurriculo();
                while (SetInstance(dr, objParametroCurriculo, false))
                {
                    lstParametroCurriculo.Add(objParametroCurriculo);
                    objParametroCurriculo = new ParametroCurriculo();
                }
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

            return lstParametroCurriculo;
        }
        #endregion

        #region CarregarParametroPorCurriculo
        /// <summary>
        /// Carrega um objeto da classe ParametroCurriculo atráves do parametro e da filial
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

        #region RecuperarValorParametroPorCurriculo
        /// <summary>
        /// Recupera o valor do parametro para um currículo
        /// </summary>
        /// <returns>objParametroCurriculo</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static string RecuperarValorParametroPorCurriculo(Enumeradores.Parametro parametro, Curriculo objCurriculo)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Parametro", SqlDbType = SqlDbType.Int, Size = 4, Value = (int)parametro },
                    new SqlParameter { ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = objCurriculo.IdCurriculo }
                };

            var obj = DataAccessLayer.ExecuteScalar(CommandType.Text, Spselectvalorparametrocurriculo, parms);
            if (obj != DBNull.Value)
            {
                return obj.ToString();
            }

            return string.Empty;
        }
        #endregion

        #endregion

    }
}