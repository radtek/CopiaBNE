//-- Data: 19/03/2013 11:02
//-- Autor: Gieyson Stelmak

using BNE.Common.Enumeradores;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace BNE.BLL
{
    public partial class ParametroFilial // Tabela: TAB_Parametro_Filial
    {

        #region Consultas

        #region Spselectparametrofilial
        private const string Spselectparametrofilial = @"
        SELECT  PAR.*
        FROM    TAB_Parametro_Filial PAR WITH(NOLOCK)
        WHERE   PAR.Idf_Parametro = @idf_Parametro
                AND PAR.Idf_Filial = @Idf_Filial";
        #endregion

        #region SpupdateVlrParametroFlgISS
        private const string SpupdateVlrParametroFlgISS = @"
        update bne.tab_parametro_filial
        set Vlr_Parametro = @Vlr_Parametro
        where Idf_Parametro = 347
	        and Idf_Filial = @Idf_Filial
        ";
        #endregion

        #region SpupdateVlrParametroTextoPersonalizado
        private const string SpupdateVlrParametroTextoPersonalizado = @"
        update bne.tab_parametro_filial
        set Vlr_Parametro = @Vlr_Parametro
        where Idf_Parametro = 348
	        and Idf_Filial = @Idf_Filial
        ";
        #endregion

        #endregion

        #region Métodos

        #region CarregarParametroPorFilial
        /// <summary>
        /// Carrega um objeto da classe ParametroFilial atráves do parametro e da filial
        /// </summary>
        /// <returns>objFuncao</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static bool CarregarParametroPorFilial(Enumeradores.Parametro parametro, Filial objFilial, out ParametroFilial objParametroFilial)
        {
            return CarregarParametroPorFilial(parametro, objFilial, out objParametroFilial, null);
        }
        public static bool CarregarParametroPorFilial(Enumeradores.Parametro parametro, Filial objFilial, out ParametroFilial objParametroFilial, SqlTransaction trans)
        {
            bool retorno = false;
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Parametro", SqlDbType = SqlDbType.Int, Size = 4, Value = (int)parametro },
                    new SqlParameter { ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = objFilial.IdFilial }
                };

            IDataReader dr;
            if (trans != null)
                dr = DataAccessLayer.ExecuteReader(trans, CommandType.Text, Spselectparametrofilial, parms);
            else
                dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spselectparametrofilial, parms);

            objParametroFilial = new ParametroFilial();
            if (SetInstance(dr, objParametroFilial))
                retorno = true;
            else
                objParametroFilial = null;

            dr.Dispose();
            dr.Close();

            return retorno;
        }
        #endregion

        #region AtualizarParametroPorFilialFlgISS
        public static void AtualizarParametroPorFilial(string flgIss, string textoPersonalizado, int idFilial) 
        {
            using (SqlConnection conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        var valorParametro = flgIss != null ? flgIss.ToString() : textoPersonalizado.ToString();
                        var sqlUpdate = flgIss != null ? SpupdateVlrParametroFlgISS : SpupdateVlrParametroTextoPersonalizado;

                        var parms = new List<SqlParameter> 
                        {
                            new SqlParameter{ ParameterName = "@Vlr_Parametro", SqlDbType = SqlDbType.VarChar, Value = valorParametro},
                            new SqlParameter{ ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Value = idFilial}
                        };

                        DataAccessLayer.ExecuteReader(CommandType.Text, sqlUpdate, parms);

                        trans.Commit();

                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        EL.GerenciadorException.GravarExcecao(ex);
                    }
                };
            };
        }
        #endregion  

        #region SolicitaInfoEstag
        /// <summary>
        /// Salva o email da empresa que quer contratar estag.
        /// </summary>
        /// <param name="mensagem"></param>
        /// <param name="idfFilial"></param>
        public static void SolicitaInfoEstag(string mensagem, int idfFilial) //passar id filial
        {
            using (var conn = new SqlConnection(BNE.BLL.DataAccessLayer.CONN_STRING))
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {

                        var filial = BLL.Filial.LoadObject(idfFilial);

                        ParametroFilial solicitinfoetag;

                        if (!ParametroFilial.CarregarParametroPorFilial(Enumeradores.Parametro.SolicitInfoEtag, filial, out solicitinfoetag))
                            solicitinfoetag = new ParametroFilial { IdFilial = idfFilial, IdParametro = (int)Enumeradores.Parametro.SolicitInfoEtag };




                        solicitinfoetag.ValorParametro = mensagem; // salva texto do email

                        solicitinfoetag.Save(trans);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                    }
                    finally
                    {
                        conn.Close();
                        trans.Dispose();
                    }
                }
            }


        }
        #endregion

        #endregion

    }
}