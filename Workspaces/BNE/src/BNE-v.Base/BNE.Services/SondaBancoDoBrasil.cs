using BNE.BLL;
using BNE.BLL.Custom;
using BNE.Services.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace BNE.Services
{
    partial class SondaBancoDoBrasil : ServiceBase
    {

        #region Propriedades
        private Thread _objThread;
        private static DateTime _dataHoraUltimaExecucao;
        //private static int DelayExecucao = Settings.Default.ControleFinanceiroDelayMinutos;
        private const string EventSourceName = "SondaBancoDoBrasil";
        #endregion

        public SondaBancoDoBrasil()
        {
            InitializeComponent();
        }

        #region Eventos
        #region OnStart
        protected override void OnStart(string[] args)
        {
            (_objThread = new Thread(new ThreadStart(Iniciar))).Start();
        }
        #endregion

        #region OnStop
        protected override void OnStop()
        {
            if (_objThread != null)
                _objThread.Abort();
        }
        #endregion
        #endregion

        #region Metodos

        #region Iniciar
        public void Iniciar()
        {
            Settings.Default.Reload();
            //DelayExecucao = Settings.Default.ControleFinanceiroDelayMinutos;

            while (true)
            {
                _dataHoraUltimaExecucao = DateTime.Now;
#if !DEBUG
                EventLogWriter.LogEvent(EventSourceName, String.Format("Iniciou agora {0}.", DateTime.Now), EventLogEntryType.Information, (int)EventID.InicioExecucao);
                EventLogWriter.LogEvent(EventSourceName, String.Format("Capturando trasações não capturadas: {0}.", DateTime.Now), EventLogEntryType.Information, (int)EventID.InicioExecucao);
#endif
                Settings.Default.Reload();

                try
                {
                    ObterPagamentosNaoLiberadosDebitoOnlineBB();
                }
                catch (Exception ex)
                {
#if !DEBUG
                    EventLogWriter.LogEvent(EventSourceName, ex.Message, EventLogEntryType.Error, (int)EventID.FimExecucao);
#endif
                    EL.GerenciadorException.GravarExcecao(ex);
                }
#if !DEBUG
                EventLogWriter.LogEvent(EventSourceName, String.Format("Terminou agora {0}.", DateTime.Now), EventLogEntryType.Information, (int)EventID.FimExecucao);    
                AjustarThread(DateTime.Now, false);
#endif
            }
        }

        #endregion

        #region Debito Online Banco do Brasil

        #region ObterPagamentosNaoLiberadosDebitoOnlineBB
        private void ObterPagamentosNaoLiberadosDebitoOnlineBB()
        {

            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        using (IDataReader dr = Transacao.ObterTransacoesDebitoOnlineNaoConfirmados())
                        {
                            while (dr.Read())
                            {
                                //EFETUAR O POST E AGUARDANDO O RETORNO
                                string respostaSonda = envioFormularioSonda(Convert.ToString(Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.ConvenioDebitoBB)), Convert.ToString(dr["Idf_Transacao"]), Convert.ToString(dr["Vlr_Documento"]), Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.formatoRetornoDebitoOnlineBB));
                                //Resposta do Sonda
                                if (!string.IsNullOrEmpty(respostaSonda) && respostaSonda.Length == 49)
                                {
                                    //Preenchimento dos parametros
                                    int refTran = Convert.ToInt32(respostaSonda.Substring(0, 17));
                                    decimal valor = Convert.ToDecimal(respostaSonda.Substring(17, 15));
                                    int idConv = Convert.ToInt32(respostaSonda.Substring(32, 6));
                                    int tpPagamento = Convert.ToInt32(respostaSonda.Substring(38, 1));
                                    string situacao = Convert.ToString(respostaSonda.Substring(39, 2));
                                    string dataPagamento = Convert.ToString(respostaSonda.Substring(41, 8));
                                    //int qtdPontos = Convert.ToInt32(respostaSonda.Substring(49, 15));

                                    //Confirmação de pagamento
                                    Transacao objTransacao = Transacao.LoadObject(Convert.ToInt32(dr["Idf_Transacao"]));
                                    string erro = string.Empty;
                                    Pagamento objPagamento;

                                    //verifica se a transac
                                    if ((objTransacao != null) && (refTran.Equals(objTransacao.IdTransacao)) && (Pagamento.CarregarPagamentoDeTransacao(objTransacao.IdTransacao, out objPagamento)))
                                    {
                                        if (falha(situacao, ref erro))//Falha
                                        {
                                            if (objTransacao.PlanoAdquirido.CompleteObject())
                                                objTransacao.PlanoAdquirido.CancelarPlanoAdquirido(null, "Transação Cancelada/Devolvida pelo Banco do Brasil", true);
                                            objTransacao.CancelamentoDebitoOnlineBB(erro,trans);
                                            EnviarEmailConfirmacao(objPagamento,erro);
                                        }
                                        else
                                        {
                                            //Altualizando os status
                                            objTransacao.AtualizarStatus(BNE.BLL.Enumeradores.StatusTransacao.Realizada,trans);
                                            objPagamento.Liberar(trans,DateTime.Now);
                                        }
                                        trans.Commit();
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        EL.GerenciadorException.GravarExcecao(ex);
                        throw;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
        }
        #endregion

        #region EnviarEmailConfirmacao
        private void EnviarEmailConfirmacao(Pagamento objPagamento, string erro)
        {
            string emailRemetente = Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.ContaPadraoEnvioEmail);
            string emailDestinatario = string.Empty;
            String corpoEmail = string.Empty;

            if (objPagamento.UsuarioFilialPerfil != null)
            {
                objPagamento.UsuarioFilialPerfil.CompleteObject();
            
                if (objPagamento.UsuarioFilialPerfil.PessoaFisica != null)
                    objPagamento.UsuarioFilialPerfil.PessoaFisica.CompleteObject();

                emailDestinatario = objPagamento.UsuarioFilialPerfil.PessoaFisica.EmailPessoa;
                corpoEmail = objPagamento.UsuarioFilialPerfil.PessoaFisica.NomePessoa +
                    ",\n Informamos que houve o seguinte erro com o seu pagamento:" + erro;

#if DEBUG
                emailDestinatario = "ruas@bne.com.br";
#endif

                MailController.Send(emailDestinatario, emailRemetente, "[DÉBITO VIA BB Para pagamento BNE]", corpoEmail);
            }
        }
        #endregion

        #region Falha
        private bool falha(string situacao, ref string erro)
        {
            Dictionary<String, String> CodigosDeFalha = new Dictionary<string, string>();
            CodigosDeFalha.Add("01", "Pagamento não autorizado/transação recusada");
            CodigosDeFalha.Add("02", "Erro no processamento da consulta");
            CodigosDeFalha.Add("03", "Pagamento não localizado");
            CodigosDeFalha.Add("10", "Campo “idConv” inválido ou nulo");
            CodigosDeFalha.Add("11", "Valor informado é inválido, nulo ou não confere com o valor registrado");
            CodigosDeFalha.Add("21", "Pagamento Web não autorizado");
            CodigosDeFalha.Add("22", "Erro no processamento da consulta");
            CodigosDeFalha.Add("23", "Erro no processamento da consulta");
            CodigosDeFalha.Add("24", "Convênio não cadastrado");
            CodigosDeFalha.Add("25", "Convênio não ativo");
            CodigosDeFalha.Add("26", "Convênio não permite debito em conta");
            CodigosDeFalha.Add("27", "Serviço inválido");
            CodigosDeFalha.Add("28", "Boleto emitido");
            CodigosDeFalha.Add("29", "Pagamento não efetuado");
            CodigosDeFalha.Add("30", "Erro no processamento da consulta");
            CodigosDeFalha.Add("99", "Operação cancelada pelo cliente");

            if (CodigosDeFalha.Keys.Contains(situacao)) //Falha
            {
                erro = CodigosDeFalha[situacao];
                return true;
            }
            return false;
        }

        #endregion

        #region EnvioFormularioSonda
        private string envioFormularioSonda(string idConv, string refTran, string valorSonda, string formato)
        {
            //Construindo envio
            string urlPost = Parametro.RecuperaValorParametro(BNE.BLL.Enumeradores.Parametro.URLPOSTBBREC3);
            string dadosDaRequisicao = string.Format("idConv={0}&refTran={1}&valorSonda={2}&formato={3}", idConv, refTran, valorSonda.Replace(",", ""), formato.ToString());
            return Regex.Replace(BNE.BLL.Custom.Helper.EfetuarRequisicaoApplicationForm(urlPost, dadosDaRequisicao, string.Empty), @"[^\d]", string.Empty);
        }
        #endregion

        #endregion

        #endregion
    }
}
