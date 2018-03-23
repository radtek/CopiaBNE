//-- Data: 06/09/2013 16:34
//-- Autor: Luan Fernandes

using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace BNE.BLL
{
	public partial class AlertaErroEnvioMensagens // Tabela: alerta.Tab_Alerta_ErroEnvioMensagens
    {
        #region M�todos

        /// <summary>
        /// Processa o retorno do encio do Email via Mandrill
        /// </summary>
        /// <param name="resultadoEnvio">Lista que retorna do Send do mandrill</param>
        /// <param name="DtInicio">Data do Inicio do Processamento</param>
        /// <param name="dtFim">Data do Fim do Processamento</param>
        /// <param name="Msgm">Corpo da mensagem</param>
        /// <param name="Idf_Curriculo">Cv com erro</param>
        /// <param name="Nome">Destinat�rio</param>
        /// <remarks>Luan Fernandes</remarks>
        public void ProcessaListRetornoEnvioEmails(List<Mandrill.EmailResult> resultadoEnvio, DateTime DtInicio, DateTime dtFim, string Msgm, int Idf_Curriculo, string Nome)
        { 
        
            using(SqlConnection conn = new SqlConnection(DataAccessLayer.CONN_NOTIFICACAO))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        foreach (Mandrill.EmailResult resultado in resultadoEnvio)
                        {
                            if (resultado.Status != Mandrill.EmailResultStatus.Sent)
                            {
                                AlertaErroEnvioMensagens erro = new AlertaErroEnvioMensagens();
                                erro.DatCadastro = DateTime.Now;
                                erro.DatPeriodoFim = dtFim;
                                erro.DatPeriodoIni = DtInicio;
                                erro.EmailDestinatario = resultado.Email;
                                erro.DescricaoErro = "Email n�o enviado "+ resultado.Status.ToString();
                                erro.DescricaoMensagem = Msgm;
                                erro.IdCurriculo = Idf_Curriculo;
                                erro.NomeDestinatario = Nome;

                                erro.Save(trans);
                            }
                        }
                        
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        EL.GerenciadorException.GravarExcecao(ex);
                        trans.Rollback();
                    }
                }
            }
        
        }

        /// <summary>
        /// Processa o retorno do encio do Email via Mandrill
        /// </summary>
        /// <param name="DtInicio">Data do Inicio do Processamento</param>
        /// <param name="dtFim">Data do Fim do Processamento</param>
        /// <param name="Msgm">Corpo da mensagem</param>
        /// <param name="Idf_Curriculo">Cv com erro</param>
        /// <param name="Nome">Destinat�rio</param>
        /// <param name="Email">Email do destinat�rio</param>
        /// <remarks>Luan Fernandes</remarks>
        public static void ProcessaListRetornoEnvioEmails(DateTime DtInicio, DateTime dtFim, string Msgm, int Idf_Curriculo, string Nome, string Email)
        {

            using (SqlConnection conn = new SqlConnection(DataAccessLayer.CONN_NOTIFICACAO))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        #region Gera o AlertaErro e grava

                        AlertaErroEnvioMensagens erro = new AlertaErroEnvioMensagens();
                        erro.DatCadastro = DateTime.Now;
                        erro.DatPeriodoFim = dtFim;
                        erro.DatPeriodoIni = DtInicio;
                        erro.EmailDestinatario = Email;
                        erro.DescricaoErro = "Email n�o enviado com Mandrill";
                        erro.DescricaoMensagem = Msgm;
                        erro.IdCurriculo = Idf_Curriculo;
                        erro.NomeDestinatario = Nome;

                        erro.Save(trans);

                        #endregion

                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        EL.GerenciadorException.GravarExcecao(ex);
                        trans.Rollback();
                    }
                }
            }
        }
        #endregion

    }
}