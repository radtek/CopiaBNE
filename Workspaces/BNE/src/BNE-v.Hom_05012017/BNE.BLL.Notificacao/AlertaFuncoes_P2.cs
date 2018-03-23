//-- Data: 23/07/2013 15:25
//-- Autor: Luan Fernandes

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BNE.BLL.Notificacao
{
    public partial class AlertaFuncoes // Tabela: alerta.Tab_Alerta_Funcoes
    {
        #region Propriedades
        #endregion

        #region Consultas

        #region SPRECUPERARALERTASEXISTENTESCURRICULO

        private const string sprecuperaralertasexistentescurriculo = @"
        SELECT Idf_Funcao, Des_Funcao, Flg_Similar, Flg_Inativo
          FROM alerta.Tab_Alerta_Funcoes WITH(NOLOCK)
         WHERE Idf_Curriculo = @IdCurriculo";

        #endregion

        #region [spDeleteTodosAlertas]
        private const string spDeleteTodosAlertas = @"delete alerta.TAB_Alerta_Cidades where Idf_Curriculo = @Idf_Curriculo";
        #endregion

        #endregion

        #region Métodos

        #region ListarFuncoesAlertaCurriculo

        public static DataTable ListarFuncoesAlertaCurriculo(int IdCurriculo)
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

        #region SalvarListaAlertaFuncoesCurriculo

        public static void SalvarListaAlertaFuncoesCurriculo(List<AlertaFuncoes> listFuncoes, SqlTransaction trans = null)
        {
            foreach (AlertaFuncoes funcao in listFuncoes)
            {
                try
                {
                    funcao.Save(trans);
                }
                catch(Exception ex)
                {
                    EL.GerenciadorException.GravarExcecao(ex, string.Format("Erro ao gravar alerta por função {0}", funcao.DescricaoFuncao ));
                }

            }

        }

        #endregion

        #region [DeletaTodosAlertas]
        /// <summary>
        /// Deleta todos os alertas de funnções para o curriculo.
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