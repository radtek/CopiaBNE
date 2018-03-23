using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace BNE.BLL
{
    public partial class Mensagem
    {
        #region Consultas

        #region SPRECUPERAEMAILS
        private const String SPRECUPERAEMAILS =
@"select 
   top ({0})
   tab.Idf_Mensagem_CS,
   tab.Dta_Envio, 
   tab.Idf_Tipo_Mensagem, 
   tab.Idf_Status_Mensagem, 
   tab.Des_Mensagem,
   tab.Des_Email_Remetente, 
   tab.Des_Email_Destino, 
   tab.Des_Assunto, 
   tab.Nme_Anexo, 
   bne.Arq_Anexo, 
   tab.Num_DDD_Celular, 
   tab.Num_Celular, 
   tab.Idf_Sistema, 
   tab.Dta_Cadastro, 
   tab.Flg_Inativo
from 
  bne.TAB_Mensagem_cs tab
  left join bne.BNE_Mensagem_CS bne on (tab.Idf_Mensagem_CS = bne.Idf_Mensagem_CS)
where 
  Idf_Tipo_Mensagem = 2 
  and Idf_Status_Mensagem in (0,1)
  and tab.Des_Email_Destino is not null
  and Len(tab.Des_Email_Destino) > 5
  and CHARINDEX('@',tab.Des_Email_Destino) > 0";
        #endregion 

        #region SPREMOVERMENSAGENS
        private const String SPREMOVERMENSAGENS =
            @"delete from bne.TAB_Mensagem_CS where Idf_Mensagem_CS in ({0})";
        #endregion 

        #region SP_MARCAR_MENSAGENS_COM_ERRO
        private const String SP_MARCAR_MENSAGENS_COM_ERRO =
            @"UPDATE bne.TAB_Mensagem_CS SET Idf_Status_Mensagem = 4 where Idf_Mensagem_CS in ({0})";
        #endregion 

        #endregion 

        #region Metodos

        #region RecuperarEmailsNaoEnviados
        /// <summary>
        /// Recupera os primeiros 500 emails não enviados
        /// </summary>
        /// <returns>Uma datatable com os emails não enviados</returns>
        public static DataTable RecuperarEmailsNaoEnviados()
        {            
            // traz a quantidade de emails que sera recuperada
            string qtdeEmailsRecuperados = Parametro.RecuperaValorParametro(Enumeradores.Parametro.QuantidadeDeEmailsRoboEnviaEmail);

            string sql = String.Format(SPRECUPERAEMAILS, qtdeEmailsRecuperados);

            return DataAccessLayer.ExecuteReaderDs(CommandType.Text, sql, new List<System.Data.SqlClient.SqlParameter>()).Tables[0];
        }
        #endregion 

        #region RemoverMensagens
        /// <summary>
        /// Remove as mensagens parametrizadas
        /// </summary>
        /// <param name="lstMensagens">A lista com os ids das mensagens a serem removidas.</param>
        public static void RemoverMensagens(List<int> lstMensagens)
        {
            List<String> lstIds = new List<string>();
            foreach (int i in lstMensagens)
            {
                lstIds.Add(Convert.ToString(i));
            }

            string sql = String.Format(SPREMOVERMENSAGENS, String.Join(",", lstIds.ToArray()));

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, sql, new List<SqlParameter>());
        }
        #endregion

        #region MarcarMensagensComErro
        /// <summary>
        /// Marca as mensagens parametrizadas com erro no envio
        /// </summary>
        /// <param name="lstMensagens">A lista com os ids das mensagens a serem marcadas.</param>
        public static void MarcarMensagensComErro(List<int> lstMensagens)
        {
            List<String> lstIds = new List<string>();
            foreach (int i in lstMensagens)
            {
                lstIds.Add(Convert.ToString(i));
            }

            string sql = String.Format(SP_MARCAR_MENSAGENS_COM_ERRO, String.Join(",", lstIds.ToArray()));

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, sql, new List<SqlParameter>());
        }
        #endregion

        #region EnvioSMSTanque
        public static void EnvioSMSTanque(string idUsuarioFilialPerfilRemetente,List<DTO.PessoaFisicaEnvioSMSTanque> listaMensagens, bool agendar = false) 
        {
            using (var objWsTanque = new BLL.BNETanqueService.AppClient()) 
            {
                List<BNETanqueService.Mensagem> listaSMS = new List<BNETanqueService.Mensagem>();

                foreach (var objMensagem in listaMensagens) 
                {
                    var mensagem = new BNETanqueService.Mensagem();

                    mensagem.ci = Convert.ToString(objMensagem.idDestinatario);
                    mensagem.np = objMensagem.nomePessoa;
                    mensagem.nc = Convert.ToDecimal(objMensagem.dddCelular.Trim() + objMensagem.numeroCelular.Trim());
                    mensagem.dm = objMensagem.mensagem;

                    listaSMS.Add(mensagem);
                }

                var receberMensagem = new BNETanqueService.InReceberMensagem 
                    {
                        l = listaSMS.ToArray(),
                        cu = idUsuarioFilialPerfilRemetente.ToString(),
                        e = agendar
                    };

                try
                {
                    var retorno = objWsTanque.ReceberMensagem(receberMensagem);
                }
                catch (Exception ex)
                {
                    EL.GerenciadorException.GravarExcecao(ex);
                }
                
            }
        } 
        #endregion

        #endregion


    }
}
