using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using BNE.BLL.Custom;

namespace BNE.BLL
{
    public class MensagemMailing
    {

        #region Consultas

        #region Spremovermensagens
        private const String Spremovermensagens =
            @"delete from bne.TAB_Mensagem_Mailing where Idf_Mensagem_CS in ({0})";
        #endregion

        #region Recuperaemails
        private const String Recuperaemails = @"
        select 
           top (@Count)
           tab.Idf_Mensagem_CS,
           tab.Dta_Envio, 
           tab.Idf_Tipo_Mensagem, 
           tab.Idf_Status_Mensagem, 
           tab.Des_Mensagem,
           tab.Des_Email_Remetente, 
           tab.Des_Email_Destino, 
           tab.Des_Assunto, 
           tab.Nme_Anexo, 
           tab.Arq_Anexo, 
           tab.Num_DDD_Celular, 
           tab.Num_Celular, 
           tab.Idf_Sistema, 
           tab.Dta_Cadastro, 
           tab.Flg_Inativo
        from 
          bne.TAB_Mensagem_Mailing tab with(nolock)
          --left join bne.BNE_Mensagem_CS bne on (tab.Idf_Mensagem_CS = bne.Idf_Mensagem_CS)
        where 
          Idf_Tipo_Mensagem = 2 
          and Idf_Status_Mensagem in (0,1)
          and tab.Des_Email_Destino is not null
          and Len(tab.Des_Email_Destino) > 5
          and CHARINDEX('@',tab.Des_Email_Destino) > 0";
        #endregion

        #region Spremovermensagensinvalidas
        private const string Spremovermensagensinvalidas = @"
        DELETE  FROM TAB_MENSAGEM_MAILING
        WHERE   DES_EMAIL_DESTINO IS NULL
                --OR CHARINDEX(' ',LTRIM(RTRIM(DES_EMAIL_DESTINO))) > 0
                OR CHARINDEX('@',DES_EMAIL_DESTINO) <= 0";
        #endregion

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

        #region Metodos

        #region RemoverMensagens
        /// <summary>
        /// Remove as mensagens parametrizadas
        /// </summary>
        /// <param name="lstMensagens">A lista com os ids das mensagens a serem removidas.</param>
        public static void RemoverMensagens(List<int> lstMensagens)
        {
            string sql = String.Format(Spremovermensagens, String.Join(",", lstMensagens.Select(i => Convert.ToString(i)).ToArray()));

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, sql, new List<SqlParameter>());
        }
        #endregion

        #region RemoverMensagensInvalidas
        /// <summary>
        /// Remove todas as mensagens com email destinatário errado
        /// </summary>
        public static void RemoverMensagensInvalidas()
        {
            DataAccessLayer.ExecuteNonQuery(CommandType.Text, Spremovermensagensinvalidas, new List<SqlParameter>());
        }
        #endregion

        #region EnviarEmails
        /// <summary>
        /// Envia os Emails não enviados
        /// Divide em 10 Threads de envio
        /// </summary>
        public static void EnviarEmailsNaoEnviados(int maxEmail)
        {
            var enviados = new List<int>();

            #region LimpaEmails
            RemoverMensagensInvalidas();
            #endregion

            #region Recupera os e-mails
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Count", SqlDbType = SqlDbType.Int, Size = 4, Value = maxEmail }
                };

            DataTable dtResult = DataAccessLayer.ExecuteReaderDs(CommandType.Text, Recuperaemails, parms).Tables[0];

            Parallel.ForEach(dtResult.AsEnumerable(), new ParallelOptions { }, row =>
            {
                try
                {
                    if (row["Arq_Anexo"] != DBNull.Value)
                    {
                        var objAnexo = new Dictionary<string, byte[]> { { Convert.ToString((string)row["Nme_Anexo"]), ((byte[])row["Arq_Anexo"]) } };

                        MailController.Send(Convert.ToString((string)row["Des_Email_Destino"]), Convert.ToString((string)row["Des_Email_Remetente"]), Convert.ToString((string)row["Des_Assunto"]), Convert.ToString((string)row["Des_Mensagem"]), objAnexo);

                        lock (enviados)
                        {
                            enviados.Add(Convert.ToInt32((int)row["Idf_Mensagem_CS"]));
                        }
                    }
                    else
                    {

                        MandrillController.Send(Convert.ToString((string)row["Des_Email_Destino"]), Convert.ToString((string)row["Des_Email_Destino"]), Convert.ToString((string)row["Des_Assunto"]), Convert.ToString((string)row["Des_Mensagem"]), Convert.ToString((string)row["Des_Email_Remetente"]), Convert.ToString((string)row["Des_Email_Remetente"]), TimeoutMail);

                        lock (enviados)
                        {
                            enviados.Add(Convert.ToInt32((int)row["Idf_Mensagem_CS"]));
                        }
                    }
                }
                catch (Exception exEnvio)
                {
                    string message;
                    EL.GerenciadorException.GravarExcecao(exEnvio, out message);
                }
            });

            #region Apaga os registros

            if (enviados.Count > 0)
                RemoverMensagens(enviados);

            #endregion

        }
            #endregion

        #endregion

        #endregion

        private static int? _timeoutMail= 300000;

        public static int? TimeoutMail
        {
            get { return MensagemMailing._timeoutMail; }
            set { MensagemMailing._timeoutMail = value; }
        }

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
        #endregion SalvarDataEnvio
    }
}
