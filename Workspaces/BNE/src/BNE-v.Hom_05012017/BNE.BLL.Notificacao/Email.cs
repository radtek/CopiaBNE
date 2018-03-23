using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BNE.BLL.Notificacao
{
    public class Email
    {
        public static void EnviarSmtpCloud(string assunto, string mensagem, string emailRemetente, string emailDestinatario)
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@assunto", SqlDbType = SqlDbType.VarChar, Size = 400, Value = assunto},
                new SqlParameter {ParameterName = "@mensagem", SqlDbType = SqlDbType.VarChar, Size = -1, Value = mensagem},
                new SqlParameter {ParameterName = "@remetente", SqlDbType = SqlDbType.VarChar, Size = 200, Value = emailRemetente},
                new SqlParameter {ParameterName = "@destinatario", SqlDbType = SqlDbType.VarChar, Size = 200, Value = emailDestinatario}
            };

            DataAccessLayer.ExecuteNonQuery(CommandType.StoredProcedure, "JornalVaga_EnviarSmtpCloud", parms);
        }

        public static void EnviarSmtpCloudCampanha(string assunto, string mensagem, string emailRemetente, string emailDestinatario)
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@assunto", SqlDbType = SqlDbType.VarChar, Size = 400, Value = assunto},
                new SqlParameter {ParameterName = "@mensagem", SqlDbType = SqlDbType.VarChar, Size = -1, Value = mensagem},
                new SqlParameter {ParameterName = "@remetente", SqlDbType = SqlDbType.VarChar, Size = 200, Value = emailRemetente},
                new SqlParameter {ParameterName = "@destinatario", SqlDbType = SqlDbType.VarChar, Size = 200, Value = emailDestinatario}
            };

            DataAccessLayer.ExecuteNonQuery(CommandType.StoredProcedure, "JornalVaga_EnviarSmtpCloudCampanha", parms);
        }

        public static void EnviarAmazonSES(string assunto, string mensagem, string emailRemetente, string emailDestinatario)
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@assunto", SqlDbType = SqlDbType.VarChar, Size = 400, Value = assunto},
                new SqlParameter {ParameterName = "@mensagem", SqlDbType = SqlDbType.VarChar, Size = -1, Value = mensagem},
                new SqlParameter {ParameterName = "@remetente", SqlDbType = SqlDbType.VarChar, Size = 200, Value = emailRemetente},
                new SqlParameter {ParameterName = "@destinatario", SqlDbType = SqlDbType.VarChar, Size = 200, Value = emailDestinatario}
            };

            DataAccessLayer.ExecuteNonQuery(CommandType.StoredProcedure, "JornalVaga_EnviarAmazonSES", parms);
        }

        public static void EnviarSendGrid(string assunto, string mensagem, string emailRemetente, string emailDestinatario, string tags = null)
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@assunto", SqlDbType = SqlDbType.VarChar, Size = 400, Value = assunto},
                new SqlParameter {ParameterName = "@mensagem", SqlDbType = SqlDbType.VarChar, Size = -1, Value = mensagem},
                new SqlParameter {ParameterName = "@remetente", SqlDbType = SqlDbType.VarChar, Size = 200, Value = emailRemetente},
                new SqlParameter {ParameterName = "@destinatario", SqlDbType = SqlDbType.VarChar, Size = 200, Value = emailDestinatario}
            };

            DataAccessLayer.ExecuteNonQuery(CommandType.StoredProcedure, "JornalVaga_EnviarMailing", parms);
        }

    }
}