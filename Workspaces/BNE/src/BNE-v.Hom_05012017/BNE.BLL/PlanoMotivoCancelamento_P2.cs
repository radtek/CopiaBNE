//-- Data: 18/07/2016 15:08
//-- Autor: Mailson

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace BNE.BLL
{
	public partial class PlanoMotivoCancelamento // Tabela: BNE_Plano_Motivo_Cancelamento
	{
        #region Consulta

        #endregion

        #region Metodos

        #region Metricas

        public static IDataReader Metricas(int idCurriculo)
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4)
            };

            parms[0].Value = idCurriculo;

            return DataAccessLayer.ExecuteReader(CommandType.StoredProcedure, "BNE.SP_Metricas_CV", parms);

        }
        #endregion
        #endregion
	}
}