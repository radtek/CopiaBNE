//-- Data: 02/03/2010 09:20
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace BNE.BLL
{
	public partial class PesquisaSalarial // Tabela: TAB_Pesquisa_Salarial
    {

        #region Consultas
        private const string SPSELECTMEDIAPORFUNCAOESTADO = @"   SELECT Vlr_Media FROM TAB_Pesquisa_Salarial PS
	                                                            INNER JOIN plataforma.TAB_Estado E ON PS.Idf_Estado = E.Idf_Estado
                                                            WHERE 
	                                                            Idf_Funcao = @Idf_Funcao AND
	                                                            E.Sig_Estado = @Sig_Estado";

        private const string SPSELECTPORFUNCAO = @"   SELECT * FROM TAB_Pesquisa_Salarial PS
                                                            WHERE 
	                                                            Idf_Funcao = @Idf_Funcao AND
	                                                            PS.Idf_Estado IS NULL";
        #endregion

        #region RecuperarMedia
        /// <summary>
        /// Método responsável por recuperar uma média salarial, lenvado em conta a funcao e o Estado.
        /// </summary>
        /// <returns>Decimal</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static decimal RecuperarMedia(int idFuncao, string siglaEstado)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Funcao", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Sig_Estado", SqlDbType.Char, 2));
            parms[0].Value = idFuncao;
            parms[1].Value = siglaEstado;

            Decimal valor = 0;

            Object obj = DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTMEDIAPORFUNCAOESTADO, parms);

            if (obj != null)
                Decimal.TryParse(obj.ToString(), out valor);

            
            return valor;
        }
        #endregion

        #region RecuperarPesquisaSalarial
        /// <summary>
        /// 
        /// </summary>
        /// <param name="idFuncao"></param>
        /// <param name="siglaEstado"></param>
        /// <param name="objPesquisaSalarial"></param>
        /// <returns></returns>
        public static bool RecuperarPesquisaSalarial(int idFuncao, out PesquisaSalarial objPesquisaSalarial)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Funcao", SqlDbType.Int, 4));
            parms[0].Value = idFuncao;

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPORFUNCAO, parms))
            {
                objPesquisaSalarial = new PesquisaSalarial();
                if (SetInstance(dr, objPesquisaSalarial))
                    return true;

                if (!dr.IsClosed)
                    dr.Close();
            }

            objPesquisaSalarial = null;
            return false;
        }
        #endregion

        #region EfetuarPesquisaSalarial
        public static DataTable EfetuarPesquisaSalarial(Funcao objFuncao, Estado objEstado,out int amostra)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Funcao", SqlDbType.VarChar, 500));
            parms.Add(new SqlParameter("@Estado", SqlDbType.Int));
            parms.Add(new SqlParameter("@Amostra", SqlDbType.Int));

            parms[0].Value = objFuncao.IdFuncao;
            parms[2].Direction = ParameterDirection.Output;

            if (objEstado != null)
            {
                objEstado.CompleteObject();
                parms[1].Value = objEstado.IdEstado;
            }
            else
                parms[1].Value = DBNull.Value;

            DataSet ds = DataAccessLayer.ExecuteReaderDs(CommandType.StoredProcedure, "p_ObterMediasSalario", parms);
            
            amostra = Convert.ToInt32(parms[2].Value);

            return ds.Tables[0];
        }
        public static DataTable EfetuarPesquisaSalarial(Funcao objFuncao, Estado objEstado, Sexo objSexo, int? idadeMin, int? idadeMax, out int amostra)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Funcao", SqlDbType.Int));
            parms.Add(new SqlParameter("@Estado", SqlDbType.Int));
            parms.Add(new SqlParameter("@Amostra", SqlDbType.Int));

            parms[0].Value = objFuncao.IdFuncao;
            parms[2].Direction = ParameterDirection.Output;

            if (objEstado != null)
            {
                objEstado.CompleteObject();
                parms[1].Value = objEstado.IdEstado;
            }
            else
                parms[1].Value = DBNull.Value;

            if (objSexo != null)
            {
                SqlParameter parm = new SqlParameter("@Sexo", SqlDbType.Char, 1);
                parm.Value = objSexo.DescricaoSexo;
                parms.Add(parm);
            }

            if (idadeMin.HasValue)
            {
                SqlParameter parm = new SqlParameter("@IdadeMin", SqlDbType.Int, 4);
                parm.Value = idadeMin.Value;
                parms.Add(parm);
            }

            if (idadeMax.HasValue)
            {
                SqlParameter parm = new SqlParameter("@IdadeMax", SqlDbType.Int, 4);
                parm.Value = idadeMax.Value;
                parms.Add(parm);
            }

            DataSet ds = DataAccessLayer.ExecuteReaderDs(CommandType.StoredProcedure, "BNE_SP_PESQUISA_SALARIAL", parms);

            amostra = Convert.ToInt32(parms[2].Value);

            return ds.Tables[0];
        }
        public static DataTable RetornarCurriculos(Funcao objFuncao, Estado objEstado, Sexo objSexo, int? idadeMin, int? idadeMax)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Funcao", SqlDbType.VarChar, 500));
            parms.Add(new SqlParameter("@Estado", SqlDbType.Char, 2));

            parms[0].Value = objFuncao.IdFuncao;

            if (objEstado != null)
                parms[1].Value = objEstado.SiglaEstado;
            else
                parms[1].Value = DBNull.Value;

            if (objSexo != null)
            {
                SqlParameter parm = new SqlParameter("@Sexo", SqlDbType.Char, 1);
                parm.Value = objSexo.DescricaoSexo;
                parms.Add(parm);
            }

            if (idadeMin.HasValue)
            {
                SqlParameter parm = new SqlParameter("@IdadeMin", SqlDbType.Int, 4);
                parm.Value = idadeMin.Value;
                parms.Add(parm);
            }

            if (idadeMax.HasValue)
            {
                SqlParameter parm = new SqlParameter("@IdadeMax", SqlDbType.Int, 4);
                parm.Value = idadeMax.Value;
                parms.Add(parm);
            }

            DataTable dt = null;
            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.StoredProcedure, "BNE_SP_PESQUISA_SALARIAL", parms))
            {
                if (dr.Read())
                    dr.NextResult();

                dt = new DataTable();
                dt.Load(dr);
            }
            return dt;
        }
        #endregion

    }
}