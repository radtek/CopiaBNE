using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BNE.BLL
{
    public partial class MensagemMailing
    {

        #region Consultas

        #region SpSalvarDataEnvio
        private const string SpSalvarDataEnvio = @"
        DELETE  BNE.TAB_MENSAGEM_MAILING 
        WHERE   Idf_Mensagem_CS = @Idf_Mensagem_CS";
        #endregion

        #region SpSalvarErro
        private const string SpSalvarErro = @"
        UPDATE  BNE.TAB_MENSAGEM_MAILING 
        SET     Idf_Status_Mensagem = 3
        WHERE   Idf_Mensagem_CS = @Idf_Mensagem_CS";
        #endregion

        #endregion

        #region SalvarDataEnvio
        public void SalvarDataEnvio(int idMensagem)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Mensagem_CS", SqlDbType = SqlDbType.Int, Size = 4, Value = idMensagem }
                };

            DataAccessLayer.ExecuteScalar(CommandType.Text, SpSalvarDataEnvio, parms);
        }
        #endregion SalvarDataEnvio

        #region SalvarErro
        public void SalvarErro(int idMensagem)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Mensagem_CS", SqlDbType = SqlDbType.Int, Size = 4, Value = idMensagem }
                };

            DataAccessLayer.ExecuteScalar(CommandType.Text, SpSalvarErro, parms);
        }
        #endregion SalvarErro
       
    }
}
