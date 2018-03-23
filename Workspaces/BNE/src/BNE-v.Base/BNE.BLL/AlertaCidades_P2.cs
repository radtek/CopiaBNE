//-- Data: 23/07/2013 15:25
//-- Autor: Luan Fernandes

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace BNE.BLL
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

        #region SalvarListaAlertaCidadesCurriculo

        public static void SalvarListaAlertaCidadesCurriculo(List<AlertaCidades> listCidades)
        {
            foreach (AlertaCidades cidade in listCidades)
            {
                cidade.Save();
            }        
        }

        #endregion

        #endregion
    }
}