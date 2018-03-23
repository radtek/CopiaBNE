//-- Data: 02/03/2010 09:20
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;
namespace BNE.BLL
{
	public partial class CurriculoEntrevista // Tabela: BNE_Curriculo_Entrevista
	{

        #region Consultas
        
        private const string SPSELECTMENSAGEM = "SELECT COUNT(Idf_Mensagem_CS) FROM BNE_Curriculo_Entrevista WITH(NOLOCK) WHERE Idf_Mensagem_CS = @Idf_Mensagem_CS";

        #endregion

        #region Métodos

        #region ExisteCurriculoEntrevista
        public static bool ExisteCurriculoEntrevista(MensagemCS objMensagemCS)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Mensagem_CS", SqlDbType.Int, 4));
            parms[0].Value = objMensagemCS.IdMensagemCS;

            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTMENSAGEM, parms)) > 0;
        }
        #endregion

        #endregion

    }
}