using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace BNE.BLL.Notificacao
{
    public partial class LogEnvioMensagem
    {
        #region Consultas

        #region SP_JA_ENVIOU_ALERTA_VIP
        private const string SP_JA_ENVIOU_ALERTA_VIP = @"SELECT TOP 1 Idf_Carta_Email FROM alerta.Log_Envio_Mensagem WHERE Idf_Carta_Email = 116 AND Dta_Cadastro BETWEEN @inicio AND @fim AND Idf_Curriculo = @idCurriculo";
        #endregion
        #endregion

        public static void Logar(string assunto, string emailRemetente, string emailDestinatario, int? idCurriculo, string codigoVagas, int idCarta)
        {
            var objLogEnvioMensagem = new LogEnvioMensagem
            {
                DesAssunto = assunto,
                CartaEmail = idCarta,
                EmlDestinatario = emailDestinatario,
                EmlRemetente = emailRemetente,
                ObsMensagem = codigoVagas,
                DataCadastro = DateTime.Now
            };
            if (idCurriculo.HasValue)
            {
                objLogEnvioMensagem.curriculo = idCurriculo.Value;
            }

            objLogEnvioMensagem.Save();
        }

        public static bool EnviarSMS(int idCurriculo)
        {

            var dataInicio = string.Format("{0} 00:00", DateTime.Now.ToShortDateString());// $"{ DateTime.Now.ToShortDateString() } 00:00";
            var dataFim = string.Format("{0} 23:59", DateTime.Now.ToShortDateString()); // $"{ DateTime.Now.ToShortDateString() } 23:59";


            var dtFim = Convert.ToDateTime(dataFim);
            var dtInicio = Convert.ToDateTime(dataInicio);

            var parms = new List<SqlParameter>
            {
                new SqlParameter { ParameterName = "@idCurriculo", SqlDbType = System.Data.SqlDbType.Int, SqlValue = idCurriculo },
                new SqlParameter { ParameterName = "@inicio", SqlDbType = System.Data.SqlDbType.DateTime, SqlValue = dtInicio},
                new SqlParameter { ParameterName = "@fim", SqlDbType = System.Data.SqlDbType.DateTime, SqlValue = dtFim},
            };

            if (Convert.ToInt32(DataAccessLayer.ExecuteScalar(System.Data.CommandType.Text, SP_JA_ENVIOU_ALERTA_VIP, parms)) > 0)
                return false;

            return true;

        }
    }
}