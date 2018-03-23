using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BNE.BLL
{
    public partial class LogEnvioMensagem
    {

        #region [Consulta]
        private const string spBuscaLog = @"
                select top 1 idf_carta_email from bne_log_envio_mensagem with(nolock) 
                where idf_carta_email in (97,99,100,101,102,103) and Eml_destinatario = @Eml_Destinatario
                order by dta_cadastro desc";

        #region [spUltimoEnvio]
        private const string spUltimoEnvio = @"
                select top 1 dta_cadastro from bne_log_envio_mensagem with(nolock) 
                where idf_carta_email = @Idf_Carta_Email and Eml_destinatario = @Eml_Destinatario
                order by dta_cadastro desc";
        #endregion
        #endregion


        #region [Metodos]

        #region RetornaIdUltimaCartaEnviadaQuemMeViu
        /// <summary>
        /// Retorna a ultima carta do quem me viu enviada para o candidato
        /// </summary>
        /// <param name="idCurriculo"></param>
        /// <returns></returns>
        public static int RetornaIdUltimaCartaEnviadaQuemMeViu(string EmlDestinatario)
        {
            var parm = new List<SqlParameter>{
                new SqlParameter{ParameterName = "@Eml_Destinatario", SqlDbType = SqlDbType.VarChar, Size = 100, Value = EmlDestinatario.Trim()}
            };
            int idLogEnvio = 0;
            using (var dr = DataAccessLayer.ExecuteReader(CommandType.Text, spBuscaLog, parm))
            {
                if (dr.Read())
                    idLogEnvio = Convert.ToInt32(dr["Idf_Carta_Email"]);
                if (!dr.IsClosed)
                    dr.Close();
            }

            return idLogEnvio;
        }
        #endregion


      
        #endregion

        public static void Logar(string assunto, string emailRemetente, string emailDestinatario, int idCurriculo, string codigoVagas, int idCarta, ref DataTable dt)
        {
            var objLogEnvioMensagem = new LogEnvioMensagem
            {
                DesAssunto = assunto,
                CartaEmail = new CartaEmail(idCarta),
                curriculo = new Curriculo(idCurriculo),
                EmlDestinatario = emailDestinatario,
                EmlRemetente = emailRemetente,
                ObsMensagem = codigoVagas
            };
            objLogEnvioMensagem.AddBulkTable(ref dt);
        }


        #region Inserção em Massa
        /// <summary>
        /// Crie uma tabela para inserir em massa.
        /// Passe um DataTable nulo para e a classe em populada. 
        /// Ela irá instanciar um novo DataTable já com colunas definidas apartir dos parâmetros sql definidos na classe.
        /// Os valores setados nas propriedades são transformados em uma linha na tabela.
        /// </summary>
        /// <param name="dt"></param>
        public void AddBulkTable(ref DataTable dt)
        {
            DataAccessLayer.AddBulkTable(ref dt, this);
        }
        /// <summary>
        /// Realiza inserção em massa.
        /// </summary>
        /// <param name="dt">Tabela criada pelo método AddBulkTable</param>
        /// <param name="trans">Transação</param>
        public static void SaveBulkTable(DataTable dt, SqlTransaction trans = null)
        {
            DataAccessLayer.SaveBulkTable(dt, "bne_log_envio_mensagem2", trans);
        }
        #endregion

        #region [UltimoEnvio]

        public static DateTime? UltimoEnvio(string EmlDestinatario, Enumeradores.CartaEmail carta)
        {
            var parm = new List<SqlParameter>{
                new SqlParameter{ParameterName = "@Eml_Destinatario", SqlDbType = SqlDbType.VarChar, Size = 100, Value = EmlDestinatario.Trim()},
                new SqlParameter{ParameterName = "@Idf_Carta_Email", SqlDbType = SqlDbType.Int, Value = (int)carta }
            };
            DateTime? data = null;
            using (var dr = DataAccessLayer.ExecuteReader(CommandType.Text, spUltimoEnvio, parm))
            {
                if (dr.Read())
                    data = Convert.ToDateTime(dr["Dta_Cadastro"]);
                if (!dr.IsClosed)
                    dr.Close();
            }

            return data;
        }
        #endregion
    }
}
