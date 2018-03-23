//-- Data: 23/07/2013 15:25
//-- Autor: Luan Fernandes

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace BNE.BLL
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
                using (DataSet ds = DataAccessLayer.ExecuteReaderDs(CommandType.Text, sprecuperaralertasexistentescurriculo, parms, "CONN_NOTIFICACAO"))
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

        public static void SalvarListaAlertaFuncoesCurriculo(List<AlertaFuncoes> listFuncoes)
        {
            foreach (AlertaFuncoes funcao in listFuncoes)
            {
                funcao.Save();
            }
        }

        #endregion

        #endregion
    }
}