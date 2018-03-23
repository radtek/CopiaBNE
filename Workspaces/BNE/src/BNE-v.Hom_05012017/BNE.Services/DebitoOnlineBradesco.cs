using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml.Linq;
using BNE.BLL;
using BNE.BLL.Custom;
using BNE.BLL.Custom.Email;
using BNE.EL;
using BNE.Services.Base.EventLog;
using BNE.Services.Properties;
using StatusTransacao = BNE.BLL.Enumeradores.StatusTransacao;

namespace BNE.Services
{
    internal partial class DebitoOnlineBradesco : BaseService
    {
        public DebitoOnlineBradesco()
        {
            InitializeComponent();
        }

        #region Propriedades
        private Thread _objThread;
        private static DateTime _dataHoraUltimaExecucao;
        private static readonly int DelayExecucao = 15;
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

                    RecuperarPagamentosDebitoOnlineNaoAprovados();

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

        #region Debito Online Bradesco

        #region RecuperarPagamentosDebitoOnlineNaoAprovados
        private void RecuperarPagamentosDebitoOnlineNaoAprovados()
        {
            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();
                using (var dr = Transacao.ObterTransacoesDebitoOnlineNaoPagasBradesco(null))
                {
                    while (dr.Read())
                    {
                        using (var trans = conn.BeginTransaction())
                        {
                            try
                            {
                                //EFETUAR O POST E AGUARDANDO O RETORNO - FAZ LEITURA DAS LINHAS
                                var retorno = envioFormularioSonda(dr["Idf_Transacao"].ToString());

                                if (string.IsNullOrEmpty(Regex.Replace(retorno, @"[^0-9]", string.Empty)))
                                {
                                    var objTransacao = Transacao.LoadObject(Convert.ToInt32(dr["Idf_Transacao"]));
                                    objTransacao.CancelamentoDebitoOnline(retorno, trans);
                                    trans.Commit();
                                    continue;
                                }

                                var numOrder = Convert.ToInt32(retorno.Substring(1, 27).TrimStart(' '));
                                var statusPagamento = retorno.Substring(60, 3);
                                var erro = retorno.Substring(64, 5);
                                var dtaRetorno =
                                    Convert.ToDateTime(retorno.Substring(40, 19).Replace("#", " "),
                                        CultureInfo.CreateSpecificCulture("pt-BR")).Ticks;
                                var dtaSistema = DateTime.Now.AddHours(-3).Ticks;
                                Pagamento objPagamento;

                                //Verifica se tem mais 3 horas e caso não tenha sido paga o status fica como cancelado 
                                //verifica se a transac
                                if (numOrder != 0 && Pagamento.CarregarPagamentoDeTransacao(numOrder, out objPagamento))
                                {
                                    var objTransacao = Transacao.LoadObject(numOrder);

                                    if ((statusPagamento == "081") ||
                                        (statusPagamento == "000" && erro.TrimStart('0') == "623"))
                                    {
                                        if (objPagamento.Liberar(trans, DateTime.Now))
                                        {
                                            objTransacao.AtualizarStatus(StatusTransacao.Realizada, trans);
                                            EventLogWriter.LogEvent("Confirmando Pagamento", EventLogEntryType.Error,
                                                Event.ErroExecucao);
                                        }
                                    }
                                    else if (dtaRetorno <= dtaSistema)
                                    {
                                        CancelamentoTransacao(objTransacao, objPagamento, erro, trans);
                                    }
                                    trans.Commit();
                                }
                            }
                            catch (Exception ex)
                            {
                                trans.Rollback();
                                GerenciadorException.GravarExcecao(ex);
                                EventLogWriter.LogEvent(ex.Message, EventLogEntryType.Error, Event.ErroExecucao);
                            }
                        }
                    }
                }
            }
        }

        private void CancelamentoTransacao(Transacao objTransacao, Pagamento objPagamento, string erro,
            SqlTransaction trans)
        {
            try
            {
                var msgErro = falha(erro.TrimStart('0'));
                objTransacao.CancelamentoDebitoOnline(msgErro, trans);
                if (!string.IsNullOrEmpty(erro.TrimStart('0')))
                    EnviarEmailConfirmacao(objPagamento, msgErro);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                GerenciadorException.GravarExcecao(ex);
                EventLogWriter.LogEvent(ex.Message, EventLogEntryType.Error, Event.ErroExecucao);
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
                    .Enviar("[DÉBITO VIA BB Para pagamento BNE]", corpoEmail,null, emailRemetente, emailDestinatario);
            }
        }
        #endregion

        #region FALHA
        private string falha(string situacao)
        {
            try
            {
                if (string.IsNullOrEmpty(situacao)) return "Não autorizado";
                var xml = XElement.Load("xml/erros_debito_online_bradesco.xml");
                var erro = xml.Elements().Where(XAttribute => xml.Attribute("codigo").Value.Equals(situacao)).First();
                return string.Concat(xml.Attribute("codigo").Value,
                    string.IsNullOrEmpty(erro.Attribute("origem").Value) ? "" : " - " + erro.Attribute("origem").Value,
                    " - ", erro.Attribute("descricao").Value);
            }
            catch (Exception ex)
            {
                GerenciadorException.GravarExcecao(ex);
                EventLogWriter.LogEvent(ex.Message, EventLogEntryType.Error, Event.ErroExecucao);
                return string.Empty;
            }
        }
        #endregion

        #region EnvioFormularioSonda
        private string envioFormularioSonda(string numOrder)
        {
            //Construindo envio

            var urlPost =
                Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.URLRecuperaTransacaoDebitoOnlineBradesco);
            var dadosDaRequisicao = string.Format("merchantid={0}&data={1}&Manager={2}&passwd={3}&NumOrder={4}",
                Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.ConvenioDebitoBradesco),
                string.Format("{0:dd/MM/yyyy}", DateTime.Now),
                Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.AdminConvenioBradesco),
                Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.PasswdConvenioBradesco),
                numOrder
                );
            return Helper.EfetuarRequisicaoApplicationForm(urlPost, dadosDaRequisicao, string.Empty);
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
                    horaParaExecucao = horaParaExecucao.AddMinutes(15);

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