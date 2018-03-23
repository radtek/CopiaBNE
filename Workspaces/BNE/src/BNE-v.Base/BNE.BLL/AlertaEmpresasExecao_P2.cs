//-- Data: 05/09/2013 11:45
//-- Autor: Luan Fernandes

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace BNE.BLL
{
	public partial class AlertaEmpresasExecao // Tabela: alerta.Tab_Alerta_EmpresasExecao
    {
        #region Consultas

        #region ListarEmpresasPorCurriculo

        private const string ListarEmpresasPorCurriculo = @"
        SELECT Idf_EmpresasExcecao,
               Idf_Curriculo,
               Idf_Empresa,
               Des_RazaoSocial_Empresa
          FROM alerta.Tab_Alerta_EmpresasExecao WITH(NOLOCK)
         WHERE Idf_Curriculo = @Idf_Curriculo";

        #endregion

        #region DELETEPORFILIAL

        private const string DELETEPORFILIAL = "DELETE FROM alerta.Tab_Alerta_EmpresasExecao WHERE Idf_Empresa = @Idf_Empresa";

        #endregion
        
        #endregion

        #region Métodos

        #region ListarEmpresasExcecao

        public static List<AlertaEmpresasExecao> ListarEmpresasExcecao(int Idf_Curriculo)
        {
            List<AlertaEmpresasExecao> lista = new List<AlertaEmpresasExecao>();
            using (SqlConnection conn = new SqlConnection(DataAccessLayer.CONN_NOTIFICACAO))
            {
                try
                {
                    conn.Open();

                    List<SqlParameter> parms = new List<SqlParameter>();
                    parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int));
                    parms[0].Value = Idf_Curriculo;

                    using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, ListarEmpresasPorCurriculo, parms, DataAccessLayer.CONN_NOTIFICACAO))
                    {
                        while (dr.Read())
                        {
                            lista.Add(AlertaEmpresasExecao.LoadObject(Convert.ToInt32(dr["Idf_EmpresasExcecao"])));
                        }
                    }
                }
                catch
                {
                    throw;
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
            return lista;
        }

        #endregion

        #region Delete
        /// <summary>
        /// Método utilizado para excluir uma instância de AlertaEmpresasExecao no banco de dados.
        /// </summary>
        /// <param name="idEmpresasExcecao">Chave do registro Idf_Filial.</param>
        /// <remarks>Luan Fernandes</remarks>
        public static void DeletePorFilial(int idFilial)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Empresa", SqlDbType.Int, 4));

            parms[0].Value = idFilial;

            using (SqlConnection conn = new SqlConnection(DataAccessLayer.CONN_NOTIFICACAO))
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, DELETEPORFILIAL, parms);
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;

                    }
                }
                conn.Close();
                conn.Dispose();
            }
        }
        #endregion
        #endregion
    }
}