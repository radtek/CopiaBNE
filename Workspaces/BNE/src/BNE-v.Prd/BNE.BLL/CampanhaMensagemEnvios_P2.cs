using BNE.BLL.Custom;
using BNE.BLL.Custom.Email;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using FormatObject = BNE.BLL.Common.FormatObject;

namespace BNE.BLL
{
    public partial class CampanhaMensagemEnvios
    {
        #region Consultas

        #region SpRetornarByPorCampanhaMensagem
        private const string SpRetornarPorIdCampanhaMensagem = @"
            SELECT 
                * 
            FROM BNE.BNE_Campanha_Mensagem_Envios WITH(NOLOCK) 
            WHERE 
                Idf_Campanha_Mensagem = @Idf_Campanha_Mensagem
        ";
        #endregion

        #endregion

        #region spCandidatosSemRetorno
        private const string spCandidatosSemRetorno = @"SELECT DISTINCT Nme_Campanha ,
  pf.Idf_pessoa_Fisica ,
  Nme_Pessoa ,
  Num_DDD_Celular,
  Num_Celular,
  Eml_Pessoa,
  qtd  
FROM ( SELECT Idf_Campanha_Tanque ,
     Cod_Identificador_Destinatario ,
     COUNT(*) qtd
    FROM  TANQUE_PRD.tanque.TAB_Conversa c WITH ( NOLOCK )
    WHERE  Dta_Inicio_Conversa > DATEADD(DAY, -1,
              CONVERT(DATE, GETDATE()))
     AND Idf_Campanha_Tanque IS NOT NULL
     AND ISNUMERIC(Cod_Identificador_Destinatario) = 1
    GROUP BY Idf_Campanha_Tanque ,
     Cod_Identificador_Destinatario
    HAVING COUNT(*) >= 3
  ) envios
  JOIN TANQUE_PRD.tanque.TAB_Campanha_Tanque ct WITH ( NOLOCK ) ON envios.Idf_Campanha_Tanque = ct.Idf_Campanha_Tanque
  JOIN BNE_IMP.BNE.BNE_Curriculo cv WITH ( NOLOCK ) ON cv.Idf_Curriculo = CONVERT(INT, envios.Cod_Identificador_Destinatario)
  JOIN BNE_IMP.BNE.TAB_Pessoa_Fisica pf WITH ( NOLOCK ) ON cv.Idf_Pessoa_Fisica = pf.Idf_Pessoa_Fisica
  outer apply (select top 1 dta_cadastro,eml_Destinatario  from BNE_Log_Envio_Mensagem lem with(nolock)
				where idf_carta_email = 91 --SMSSemRetorno
				and  Eml_destinatario = eml_pessoa
               order by dta_cadastro desc) logEmail
  LEFT JOIN ( SELECT Idf_Campanha_Tanque ,
       Cod_Identificador_Destinatario
     FROM TANQUE_PRD.tanque.TAB_Mensagem m WITH ( NOLOCK )
       JOIN TANQUE_PRD.tanque.TAB_Conversa c WITH ( NOLOCK ) ON m.Idf_Conversa = c.Idf_Conversa
     WHERE Flg_Resposta = 1
       AND Dta_Inicio_Conversa > DATEADD(DAY, -1,
                 CONVERT(DATE, GETDATE()))
      ) respostas ON envios.Cod_Identificador_Destinatario = respostas.Cod_Identificador_Destinatario
         AND envios.Idf_Campanha_Tanque = respostas.Idf_Campanha_Tanque
WHERE respostas.Idf_Campanha_Tanque IS NULL
	  AND (logEmail.dta_cadastro is null or datediff(day, LogEmail.dta_cadastro, getdate()) > 7 )";
        #endregion

        #region Metodos
        
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
            DataAccessLayer.SaveBulkTable(dt, "BNE_Campanha_Mensagem_Envios", trans);
        }
        #endregion

        #region RetornarPorIdCampanhaMensagem
        public static List<CampanhaMensagemEnvios> RetornarPorIdCampanhaMensagem(int IdCampanhaMensagem)
        {
            List<SqlParameter> parms = new List<SqlParameter>{
                new SqlParameter() { ParameterName = "@Idf_Campanha_Mensagem", SqlDbType = SqlDbType.Int, Size = 8, Value = IdCampanhaMensagem }
            };

            List<CampanhaMensagemEnvios> lstCampanhaMensagemEnvios = new List<CampanhaMensagemEnvios>();
            CampanhaMensagemEnvios objCampanhaMensagemEnvios;

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SpRetornarPorIdCampanhaMensagem, parms, DataAccessLayer.CONN_STRING))
            {
                while (dr.Read())
                {
                    objCampanhaMensagemEnvios = new CampanhaMensagemEnvios();
                    if (SetInstanceNotDipose(dr, objCampanhaMensagemEnvios))
                        lstCampanhaMensagemEnvios.Add(objCampanhaMensagemEnvios);
                }
            }
            return lstCampanhaMensagemEnvios;
        }
        #endregion

        #region CandidatosSemRetorno
        /// <summary>
        /// Enviar Email para candidatos que não deram retorno das sms
        /// </summary>
        public static void CandidatosSemRetorno()
        {
            string assunto;
            //pegar carta para o Email
            string layout =
                CartaEmail.RetornarConteudoBNE(BLL.Enumeradores.CartaEmail.SMSSemRetorno, out assunto);

            var emailRemetente =
                Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.ContaPadraoEnvioEmail);

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, spCandidatosSemRetorno, null))
            {
                while (dr.Read())
                {
                    try
                    {
                        #region CorpoEmail

                        PessoaFisica objPessoaFisica = PessoaFisica.LoadObject(Convert.ToInt32(dr["Idf_Pessoa_Fisica"]));

                        #region [Link Botão Sim]

                        string BtnSim = "{0}/logar/{1}/?revisar=true&utm_source=cadastroviasms&utm_medium=SMS&utm_campaign=atualizartelefoneSMS";
                        BtnSim = string.Format(BtnSim, Helper.RecuperarURLAmbiente(), LoginAutomatico.GerarHashAcessoLogin(objPessoaFisica.CPF,
                           objPessoaFisica.DataNascimento, String.Format("/{0}", Rota.RecuperarURLRota(Enumeradores.RouteCollection.CadastroCurriculoMini))));

                        #endregion

                        #region [Link Botão Não]

                        string BtnNao = "{0}/logar/{1}/?utm_source=conferircadastrosms&utm_medium=SMS&utm_campaign=atualizartelefoneSMS";
                        BtnNao = string.Format(BtnNao, Helper.RecuperarURLAmbiente(), LoginAutomatico.GerarHashAcessoLogin(objPessoaFisica.CPF,
                               objPessoaFisica.DataNascimento, String.Format("/{0}", Rota.RecuperarURLRota(Enumeradores.RouteCollection.CadastroCurriculoMini))));

                        #endregion

                        var parametro = new
                        {
                            Nome = dr["Nme_Pessoa"].ToString(),
                            Telefone = Helper.FormatarTelefone(dr["Num_DDD_Celular"].ToString(), dr["Num_Celular"].ToString()),
                            Email = dr["Eml_Pessoa"].ToString(),
                            btnSim = BtnSim,
                            btnNao = BtnNao
                        };


                        string mensagem = FormatObject.ToString(parametro, layout);
                        #endregion

                        if (Validacao.ValidarEmail(dr["Eml_Pessoa"].ToString()))
                        {
                            //Enviar E-mail para o candidato
                            EmailSenderFactory.Create(TipoEnviadorEmail.Fila)
                                .Enviar(assunto, mensagem,BLL.Enumeradores.CartaEmail.SMSSemRetorno, emailRemetente,
                                    dr["Eml_Pessoa"].ToString());
                        }
                    }
                       

                    catch (Exception ex)
                    {
                        string errorMessage;
                        EL.GerenciadorException.GravarExcecao(ex, out errorMessage, "CampanhaTanqueSMSSemRetorno");
                    }

                }
            }
        }
        #endregion

        #endregion
    }
}
