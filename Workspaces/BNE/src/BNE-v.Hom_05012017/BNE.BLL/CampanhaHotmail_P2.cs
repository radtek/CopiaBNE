//-- Data: 18/04/2016 16:51
//-- Autor: mailson

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace BNE.BLL
{
	public partial class CampanhaHotmail // Tabela: TAB_Campanha_Hotmail
	{

        #region Consultas
        private const string spAtualizar = @"update bne.tab_campanha_hotmail set flg_enviado = 1 where idf_Curriculo = @Idf_Curriculo ";


        #endregion
        #region AtualizarEnvio
       /// <summary>
       /// 
       /// </summary>
       /// <param name="EmailDestinatario"></param>
       /// <returns></returns>
        public static bool AtualizarEnvio(int idCurriculo)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));
            parms[0].Value = idCurriculo;

            try
            {
                DataAccessLayer.ExecuteNonQuery(CommandType.Text, spAtualizar, parms);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
            
        }
        #endregion
	}
}