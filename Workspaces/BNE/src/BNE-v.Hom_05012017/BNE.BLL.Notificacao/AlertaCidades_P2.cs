//-- Data: 23/07/2013 15:25
//-- Autor: Luan Fernandes

using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BNE.BLL.Notificacao
{
    public partial class AlertaCidades // Tabela: alerta.Tab_Alerta_Cidades
    {
        #region Propriedades



        #endregion

        #region Consultas

        #region SPRECUPERARALERTASEXISTENTESCURRICULO

        private const string sprecuperaralertasexistentescurriculo = @"
        SELECT Idf_Cidade, Nme_Cidade, Sig_Estado, Flg_Inativo
          FROM alerta.Tab_Alerta_Cidades WITH(NOLOCK)
         WHERE Idf_Curriculo = @IdCurriculo";

        #endregion

        #region [spDeleteTodosAlertas]
        private const string spDeleteTodosAlertas = @"delete alerta.TAB_Alerta_Cidades where Idf_Curriculo = @Idf_Curriculo";
        #endregion

        #endregion

        #region Métodos

        #region ListarCidadesAlertaCurriculo

        public static DataTable ListarCidadesAlertaCurriculo(int IdCurriculo)
        {
            List<SqlParameter> parms = new List<SqlParameter>();

            parms.Add(new SqlParameter("@IdCurriculo", SqlDbType.Int, 8));

            parms[0].Value = IdCurriculo;

            DataTable dt = null;
            try
            {
                using (DataSet ds = DataAccessLayer.ExecuteReaderDs(CommandType.Text, sprecuperaralertasexistentescurriculo, parms, DataAccessLayer.CONN_STRING))
                {
                    dt = ds.Tables[0];
                }
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
            }

            return dt;
        }

        #endregion

        #region SalvarListaAlertaCidadesCurriculo

        public static void SalvarListaAlertaCidadesCurriculo(List<AlertaCidades> listCidades)
        {
            using (SqlConnection conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        foreach (AlertaCidades cidade in listCidades)
                            cidade.Save(trans);

                        trans.Commit();
                    }
                    catch (System.Exception ex)
                    {
                        trans.Rollback();
                        EL.GerenciadorException.GravarExcecao(ex, "Gravar alerta de Cidade, tela de alerta de vagas");
                        throw;
                    }

                }
            }
        }

        #endregion

        #region [DeletaTodosAlertas]
        /// <summary>
        /// Deleta todos os resgistros de alerta para Curriculo.
        /// </summary>
        /// <param name="idCurriculo"></param>
        public static void DeletaTodosAlertas(int idCurriculo)
        {
             var parametros = new List<SqlParameter>
             {
                 new SqlParameter { ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Value = idCurriculo }
             };

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, spDeleteTodosAlertas, parametros);
        }
        #endregion

        #endregion
    }
}