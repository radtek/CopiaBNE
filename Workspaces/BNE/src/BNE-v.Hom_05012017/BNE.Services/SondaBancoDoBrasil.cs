using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using BNE.BLL;
using BNE.BLL.Custom;
using BNE.BLL.Custom.Email;
using BNE.EL;
using BNE.Services.Base.EventLog;
using BNE.Services.Properties;
using StatusTransacao = BNE.BLL.Enumeradores.StatusTransacao;

namespace BNE.Services
{
    internal partial class SondaBancoDoBrasil : BaseService
    {
        public SondaBancoDoBrasil()
        {
            InitializeComponent();
        }

        #region Propriedades
        private Thread _objThread;
        private static DateTime _dataHoraUltimaExecucao;
        private static readonly int DelayExecucao = 5;
        #endregion

        #region Eventos

        #region OnStart
        protected override void OnStart(string[] args)
        {
            (_objThread = new Thread(Iniciar)).Start();
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
            try
            {
                Settings.Default.Reload();
                AjustarThread(DateTime.Now, true);

                while (true)
                {
                    _dataHoraUltimaExecucao = DateTime.Now;
                    EventLogWriter.LogEvent(string.Format("Iniciou agora {0}.", DateTime.Now),
                        EventLogEntryType.Information, Event.InicioExecucao);

                    ObterPagamentosNaoLiberadosDebitoOnlineBB();

                    RecuperarPagamentosDebitoOnlineNaoEnviados();

                    Settings.Default.Reload();
                    EventLogWriter.LogEvent(string.Format("Terminou agora {0}.", DateTime.Now),
                        EventLogEntryType.Information, Event.FimExecucao);
                    AjustarThread(DateTime.Now, false);
                }
            }
            catch (Exception ex)
            {
                string message;
                var id = GerenciadorException.GravarExcecao(ex, out message);
                if (string.IsNullOrEmpty(message))
                    return;
                message = string.Format("{0} - {1}", id, message);
                EventLogWriter.LogEvent(message, EventLogEntryType.Error, Event.ErroExecucao);
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

                using (var dr = Transacao.ObterTransacoesDebitoOnlineNaoConfirmados(StatusTransacao.Enviada))
                {
                    while (dr.Read())
                    {
                        using (var trans = conn.BeginTransaction())
                        {
                            try
                            {
                                //EFETUAR O POST E AGUARDANDO O RETORNO
                                var respostaSonda =
                                    envioFormularioSonda(
                                        Convert.ToString(
                                            Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.ConvenioDebitoBB)),
                                        Convert.ToString(dr["Idf_Transacao"]), Convert.ToString(dr["Vlr_Documento"]),
                                        Parametro.RecuperaValorParametro(
                                            BLL.Enumeradores.Parametro.formatoRetornoDebitoOnlineBB));
                                //Resposta do Sonda
                                if (!string.IsNullOrEmpty(respostaSonda) && respostaSonda.Length == 49)
                                {
                                    //Preenchimento dos parametros
                                    var refTran = Convert.ToInt32(respostaSonda.Substring(0, 17));
                                    var situacao = Convert.ToString(respostaSonda.Substring(39, 2));

                                    //Confirmação de pagamento
                                    var objTransacao = Transacao.LoadObject(Convert.ToInt32(dr["Idf_Transacao"]));
                                    var erro = string.Empty;
                                    Pagamento objPagamento;

                                    //verifica se a transac
                                    if ((objTransacao != null) && refTran.Equals(objTransacao.IdTransacao) &&
                                        Pagamento.CarregarPagamentoDeTransacao(objTransacao.IdTransacao,
                                            out objPagamento))
                                    {
                                        if (falha(situacao, ref erro)) //Falha
                                        {
                                            objTransacao.CancelamentoDebitoOnline(erro, trans);
                                            EnviarEmailConfirmacao(objPagamento, erro);
                                        }
                                        else
                                        {
                                            //Altualizando os status
                                            if (objPagamento.Liberar(trans, DateTime.Now))
                                                objTransacao.AtualizarStatus(StatusTransacao.Realizada, trans);
                                        }
                                        trans.Commit();
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                trans.Rollback();
                                GerenciadorException.GravarExcecao(ex);
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region RecuperarPagamentosDebitoOnlineNaoEnviados
        private void RecuperarPagamentosDebitoOnlineNaoEnviados()
        {
            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();
                using (var dr = Transacao.ObterTransacoesDebitoOnlineNaoConfirmados(StatusTransacao.NaoEnviada))
                {
                    while (dr.Read())
                    {
                        using (var trans = conn.BeginTransaction())
                        {
                            try
                            {
                                //EFETUAR O POST E AGUARDANDO O RETORNO
                                var respostaSonda =
                                    envioFormularioSonda(
                                        Convert.ToString(
                                            Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.ConvenioDebitoBB)),
                                        Convert.ToString(dr["Idf_Transacao"]), Convert.ToString(dr["Vlr_Documento"]),
                                        Parametro.RecuperaValorParametro(
                                            BLL.Enumeradores.Parametro.formatoRetornoDebitoOnlineBB));
                                //Resposta do Sonda
                                var numOrder = Convert.ToInt32(respostaSonda.Substring(0, 17));
                                var situacao = Convert.ToString(respostaSonda.Substring(39, 2));
                                var erro = string.Empty;
                                Pagamento objPagamento;
                                //Verifica se tem mais 3 horas e caso não tenha sido paga o status fica como cancelado 
                                //verifica se a transac
                                if (numOrder != 0 && Pagamento.CarregarPagamentoDeTransacao(numOrder, out objPagamento))
                                {
                                    var objTransacao = Transacao.LoadObject(numOrder);
                                    if (objTransacao.DataCadastro <= DateTime.Now.AddHours(-1))
                                    {
                                        if (falha(situacao, ref erro)) //Falha
                                        {
                                            objTransacao.CancelamentoDebitoOnline(erro, trans);
                                        }
                                        else
                                        {
                                            //Altualizando os status
                                            if (objPagamento.Liberar(trans, DateTime.Now))
                                                objTransacao.AtualizarStatus(StatusTransacao.Realizada, trans);
                                        }
                                    }
                                    trans.Commit();
                                }
                            }
                            catch (Exception ex)
                            {
                                trans.Rollback();
                                GerenciadorException.GravarExcecao(ex);
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region EnviarEmailConfirmacao
        private void EnviarEmailConfirmacao(Pagamento objPagamento, string erro)
        {
            var emailRemetente = Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.ContaPadraoEnvioEmail);
            var emailDestinatario = string.Empty;
            var corpoEmail = string.Empty;

            if (objPagamento.UsuarioFilialPerfil != null)
            {
                objPagamento.UsuarioFilialPerfil.CompleteObject();

                if (objPagamento.UsuarioFilialPerfil.PessoaFisica != null)
                    objPagamento.UsuarioFilialPerfil.PessoaFisica.CompleteObject();

                emailDestinatario = objPagamento.UsuarioFilialPerfil.PessoaFisica.EmailPessoa;
                corpoEmail = objPagamento.UsuarioFilialPerfil.PessoaFisica.NomePessoa +
                             ",\n Informamos que houve o seguinte erro com o seu pagamento:" + erro;

#if DEBUG
                emailDestinatario = "rodrigobandini@bne.com.br";
#endif

                EmailSenderFactory.Create(TipoEnviadorEmail.Fila)
                    .Enviar("[DÉBITO VIA BB Para pagamento BNE]", corpoEmail, null, emailRemetente, emailDestinatario);
            }
        }
        #endregion

        #region Falha
        private bool falha(string situacao, ref string erro)
        {
            var CodigosDeFalha = new Dictionary<string, string>();
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
            var urlPost = Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.URLPOSTBBREC3);
            var dadosDaRequisicao = string.Format("idConv={0}&refTran={1}&valorSonda={2}&formato={3}", idConv, refTran,
                valorSonda.Replace(",", "").Replace(".", "").TrimStart('0'), formato);
            return Regex.Replace(Helper.EfetuarRequisicaoApplicationForm(urlPost, dadosDaRequisicao, string.Empty),
                @"[^\d]", string.Empty);
        }
        #endregion

        #endregion

        #region AjustarThread
        private void AjustarThread(DateTime horaAtual, bool primeiraExecucao)
        {
            if (primeiraExecucao)
            {
                var horaParaExecucao = DateTime.Now;

                if (horaParaExecucao.Subtract(horaAtual).TotalMilliseconds < 0)
                    horaParaExecucao = horaParaExecucao.AddMinutes(5);

                var tempoParaExecutar = horaParaExecucao - horaAtual;

                EventLogWriter.LogEvent(
                    string.Format("Está aguardando {0} para sua iniciar sua execução.", tempoParaExecutar),
                    EventLogEntryType.Information, Event.AjusteExecucao);
                Thread.Sleep((int) tempoParaExecutar.TotalMilliseconds);
            }
            else
            {
                var tempoTotalExecucao = horaAtual - _dataHoraUltimaExecucao;
                var delay = new TimeSpan(0, DelayExecucao, 0);
                if ((int) delay.TotalMilliseconds - (int) tempoTotalExecucao.TotalMilliseconds > 0)
                {
                    EventLogWriter.LogEvent(
                        string.Format(
                            "Está aguardando {0} para sua próxima execução, pois levou {1} para concluir a excução atual.",
                            delay - tempoTotalExecucao, tempoTotalExecucao), EventLogEntryType.Information,
                        Event.AjusteExecucao);
                    Thread.Sleep((int) delay.TotalMilliseconds - (int) tempoTotalExecucao.TotalMilliseconds);
                }
                else
                    EventLogWriter.LogEvent(
                        string.Format(
                            "Vai reiniciar agora {0} pois sua última execução foi às {1}, seu tempo de execução foi de {2} e seu delay é de {3}.",
                            DateTime.Now, _dataHoraUltimaExecucao, tempoTotalExecucao, (int) delay.TotalMilliseconds),
                        EventLogEntryType.Warning, Event.WarningAjusteExecucao);
            }
        }
        #endregion

        #endregion
    }
}