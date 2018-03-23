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
         WHERE Idf_Curriculo = @IdCurriculo and Flg_Inativo = 0";

        #endregion

        #region [spDeleteTodosAlertas]
        private const string spDeleteTodosAlertas = @"delete alerta.TAB_Alerta_Cidades where Idf_Curriculo = @Idf_Curriculo";
        #endregion

        #region [spExisteAlerta]
        private const string spExisteAlerta = @"select top 1 idf_curriculo_cidade from alerta.tab_alerta_cidades with(nolock)
                where idf_curriculo = @Idf_Curriculo and idf_Cidade = @Idf_Cidade";
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
            foreach (AlertaCidades cidade in listCidades)
            {
                cidade.Save();
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

        #region [ExisteAlerta]
        public static bool ExisteAlerta(int idCurriculo, int idCidade)
        {
            List<SqlParameter> parms = new List<SqlParameter>
            {
                new SqlParameter {ParameterName ="@Idf_Curriculo", SqlDbType = SqlDbType.Int, Value = idCurriculo },
                new SqlParameter {ParameterName = "@Idf_Cidade", SqlDbType = SqlDbType.Int, Value = idCidade }
            };
            using (var dr = DataAccessLayer.ExecuteReader(CommandType.Text, spExisteAlerta, parms))
            {
                if (dr.Read())
                    return true;
            }
            return false;
        }
        #endregion

        #endregion
    }
}