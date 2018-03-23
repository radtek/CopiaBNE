//-- Data: 09/03/2010 17:06
//-- Autor: Gieyson Stelmak

using BNE.StorageManager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace BNE.BLL
{
    public partial class PessoaFisicaComplemento // Tabela: TAB_Pessoa_Fisica_Complemento
    {
        #region Consultas

        const string SP_ANEXO_POR_CPF = @"SELECT Nme_Anexo 
                                            FROM BNE.TAB_Pessoa_Fisica_Complemento pfc
                                            JOIN BNE.TAB_Pessoa_Fisica pf ON pfc.Idf_Pessoa_Fisica = pf.Idf_Pessoa_Fisica
                                            WHERE pf.Num_CPF = @Num_CPF";
        const string SP_ANEXO_POR_PESSOA_FISICA = "SELECT Arq_Anexo, Nme_Anexo FROM BNE.TAB_Pessoa_Fisica_Complemento WHERE Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica";
        const string SP_CURRICULOS_COM_ANEXO = @"SELECT TOP 10000 pf.Idf_Pessoa_Fisica, pf.Num_CPF,pfc.Nme_Anexo, pfc.Arq_Anexo FROM BNE.BNE_Curriculo AS cv WITH(NOLOCK)
                                                JOIN BNE.TAB_Pessoa_Fisica AS pf WITH(NOLOCK) ON pf.Idf_Pessoa_Fisica = cv.Idf_Pessoa_Fisica
                                                JOIN BNE.TAB_Pessoa_Fisica_Complemento AS pfc WITH(NOLOCK)
                                                ON pfc.Idf_Pessoa_Fisica = cv.Idf_Pessoa_Fisica
                                            	LEFT JOIN BNE.BNE_Log_Migracao_Anexo_CV AS CVlog WITH(NOLOCK) ON CVlog.Idf_Pessoa_Fisica = pfc.Idf_Pessoa_Fisica
                                                WHERE pfc.Arq_Anexo IS NOT NULL AND CVlog.Idf_Log_Migracao_Anexo_CV IS NULL";

        const string SP_INSERT_HISTORICO_MIGRACAO = @"INSERT INTO BNE_Log_Migracao_Anexo_CV (Idf_Pessoa_Fisica,Nme_Anexo)values(@Idf_Pessoa_Fisica,@Nme_Anexo)";

        #endregion


        #region Métodos

        #region CarregarTodosAnexos
        public static IDataReader CarregarTodosAnexos()
        {
            return DataAccessLayer.ExecuteReader(CommandType.Text, SP_CURRICULOS_COM_ANEXO, null);
        }
        #endregion

        #region AtualizarTabelaMigracaoAnexo
        public static bool AtualizarTabelaMigracaoAnexo(int idPessoaFisica, string nomeAnexo)
        {
            bool retorno = false;

            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Nme_Anexo", SqlDbType.VarChar, 200));

            parms[0].Value = idPessoaFisica;
            parms[1].Value = nomeAnexo;

            using (SqlConnection conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(CommandType.Text, SP_INSERT_HISTORICO_MIGRACAO, parms);
                        cmd.Parameters.Clear();
                        trans.Commit();
                        retorno = true;
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return retorno;
        }
        #endregion

        #region CarregarAnexoPorPessoaFisicaDoStorage
        /// <summary>
        /// Método utilizado para retornar um anexo do Curriculo do Storage Manager
        /// </summary>
        /// <param name="idPessoaFisica"></param>
        /// <param name="nomeArquivo"></param>
        /// <returns></returns>
        public static byte[] CarregarAnexoPorPessoaFisicaDoStorage(int idPessoaFisica, out string nomeArquivo){

            byte[] retorno = null;
            nomeArquivo = null;
            PessoaFisica objPessoa = PessoaFisica.LoadObject(idPessoaFisica);

            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int, 4));
            parms[0].Value = idPessoaFisica;

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SP_ANEXO_POR_PESSOA_FISICA, parms))
            {
                if (dr.Read())
                {
                    nomeArquivo = dr["Nme_Anexo"].ToString();
                }

                if (!dr.IsClosed)
                    dr.Close();
            }

            retorno = StorageManager.CarregarArquivo("curriculos", objPessoa.CPF + "_" + nomeArquivo);
            
            return retorno;
        }
        #endregion

        #region CarregarAnexoPorCPFDoStorage
        /// <summary>
        /// Método utilizado para retornar um anexo do Curriculo do Storage Manager
        /// </summary>
        /// <param name="idPessoaFisica"></param>
        /// <param name="nomeArquivo"></param>
        /// <returns></returns>
        public static byte[] CarregarAnexoPorCPFDoStorage(decimal numCpf, out string nomeArquivo)
        {
            byte[] retorno = null;
            nomeArquivo = null;

            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Num_CPF", SqlDbType.Decimal, 11));
            parms[0].Value = numCpf;

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SP_ANEXO_POR_CPF, parms))
            {
                if (dr.Read())
                {
                    nomeArquivo = dr["Nme_Anexo"].ToString();
                }

                if (!dr.IsClosed)
                    dr.Close();
            }

            retorno = StorageManager.CarregarArquivo("curriculos", numCpf + "_" + nomeArquivo);

            return retorno;
        }
        #endregion

        #region CarregarAnexoPorPessoaFisica
        /// <summary>
        /// Método utilizado para retornar uma instância de Pessoa Fisica Complemento a partir do banco de dados.
        /// </summary>
        /// <param name="codigo">identificador da pessoa fisica</param>
        /// <param name="objPessoaFisicaComplemento">objeto de saída </param>
        /// <returns></returns>
        public static byte[] CarregarAnexoPorPessoaFisica(int idPessoaFisica, out string nomeArquivo)
        {
            byte[] retorno = null;
            nomeArquivo = null;

            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int, 4));
            parms[0].Value = idPessoaFisica;

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SP_ANEXO_POR_PESSOA_FISICA, parms))
            {
                if (dr.Read() && dr["Arq_Anexo"] != DBNull.Value){
                    retorno = (byte[])(dr["Arq_Anexo"]);
                    nomeArquivo = dr["Nme_Anexo"].ToString();
                }

                if (!dr.IsClosed)
                    dr.Close();
            }
            
            return retorno;
        }
        #endregion

        #region CarregarPorPessoaFisica
        /// <summary>
        /// Método utilizado para retornar uma instância de Pessoa Fisica Completo a partir do banco de dados.
        /// </summary>
        /// <param name="codigo">identificador da pessoa fisica</param>
        /// <param name="objPessoaFisicaComplemento">objeto de saída </param>
        /// <returns></returns>
        public static bool CarregarPorPessoaFisica(int idPessoaFisica, out PessoaFisicaComplemento objPessoaFisicaComplemento)
        {
            using (IDataReader dr = LoadDataReader(idPessoaFisica))
            {
                objPessoaFisicaComplemento = new PessoaFisicaComplemento();
                if (SetInstance(dr, objPessoaFisicaComplemento))
                    return true;

                if (!dr.IsClosed)
                    dr.Close();
            }
            objPessoaFisicaComplemento = null;
            return false;
        }
        #endregion

        #region SalvarIntegracao
        public void SalvarIntegracao(DateTime dtaAlteracao)
        {
            if (!this._persisted)
                InsertIntegracao(dtaAlteracao);
            else
                UpdateIntegracao(dtaAlteracao);
        }
        #endregion

        #region InsertIntegracao
        public void InsertIntegracao(DateTime dtaAlteracao)
        {
            using (SqlConnection conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        List<SqlParameter> parms = GetParameters();
                        SetParameters(parms);

                        //sincroniza a Data de Alteração do Registro para evitar trashing de sincronismo.
                        parms.Find(ParametroDataAlteracao).Value = dtaAlteracao;

                        SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
                        cmd.Parameters.Clear();
                        this._persisted = true;
                        this._modified = false;

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }
        #endregion

        #region UpdateIntegracao
        public void UpdateIntegracao(DateTime dtaAlteracao)
        {
            using (SqlConnection conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        List<SqlParameter> parms = GetParameters();
                        SetParameters(parms);

                        //sincroniza a Data de Alteração do Registro para evitar trashing de sincronismo.
                        parms.Find(ParametroDataAlteracao).Value = dtaAlteracao;

                        DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPUPDATE, parms);
                        this._modified = false;

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }
        #endregion

        #region ParametroDataAlteracao
        private static bool ParametroDataAlteracao(SqlParameter parm)
        {
            if (parm.ParameterName == "@Dta_Alteracao")
                return true;

            return false;
        }
        #endregion

        #endregion

        #region ListarTodasComAnexo
        public static IEnumerable<PessoaFisicaComplemento> ListarTodasComAnexo(int idPessoaFisica, int top)
        {
            var parms = new List<SqlParameter>
				{
					new SqlParameter { ParameterName = "@Idf_Pessoa_Fisica", SqlDbType = SqlDbType.Int, Value = idPessoaFisica },
					new SqlParameter { ParameterName = "@TOP", SqlDbType = SqlDbType.Int, Value = top }
				};

            var sp = "SELECT    TOP(@TOP) Nme_Anexo, Arq_Anexo, Idf_Pessoa_Fisica " +
                     "FROM      bne.TAB_Pessoa_Fisica_Complemento WITH(NOLOCK) " +
                     "WHERE     Nme_Anexo IS NOT NULL AND Arq_Anexo IS NOT NULL " +
                     "          AND Idf_Pessoa_Fisica > @Idf_Pessoa_Fisica " +
                     "ORDER BY  Idf_Pessoa_Fisica";

            var retorno = DataAccessLayer.ExecuteReaderDs(CommandType.Text, sp, parms).Tables[0];

            return from DataRow ret in retorno.Rows select new PessoaFisicaComplemento { NomeAnexo = ret["Nme_Anexo"].ToString(), ArquivoAnexo = (byte[])ret["Arq_Anexo"], PessoaFisica = new PessoaFisica(Convert.ToInt32(ret["Idf_Pessoa_Fisica"])) };
        }
        #endregion

        #region ListarMaiorId
        public static int ListarMaiorId()
        {
            var sp = "SELECT    TOP(1) Idf_Pessoa_Fisica " +
                     "FROM      bne.TAB_Pessoa_Fisica_Complemento WITH(NOLOCK) " +
                     "WHERE     Nme_Anexo IS NOT NULL AND Arq_Anexo IS NOT NULL " +
                     "ORDER BY Idf_Pessoa_Fisica DESC";

            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, sp, null));
        }
        #endregion

    }
}