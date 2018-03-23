using System;
using System.Diagnostics;
using System.Threading;
using BNE.BLL;
using BNE.BLL.Integracoes.WFat;
using BNE.EL;
using BNE.Services.Base.EventLog;
using BNE.Services.Properties;

namespace BNE.Services
{
    /// <summary>
    /// </summary>
    internal partial class ControleFinanceiro : BaseService
    {
        #region Construtor
        public ControleFinanceiro()
        {
            InitializeComponent();
        }
        #endregion

        #region Propriedades
        private Thread _objThread;
        private static DateTime _dataHoraUltimaExecucao;
        private static int DelayExecucao = Settings.Default.ControleFinanceiroDelayMinutos;
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
            Settings.Default.Reload();
            DelayExecucao = Settings.Default.ControleFinanceiroDelayMinutos;

            while (true)
            {
                _dataHoraUltimaExecucao = DateTime.Now;
#if !DEBUG
                EventLogWriter.LogEvent(String.Format("Iniciou agora {0}.", DateTime.Now), EventLogEntryType.Information, Event.InicioExecucao);
                EventLogWriter.LogEvent(String.Format("Capturando trasações não capturadas: {0}.", DateTime.Now), EventLogEntryType.Information, Event.InicioExecucao);
#endif
                Settings.Default.Reload();

#if !DEBUG
                EventLogWriter.LogEvent(String.Format("Obtendo notas: {0}.", DateTime.Now), EventLogEntryType.Information, Event.InicioExecucao);
#endif
                try
                {
                    ObterNotas();
                }
                catch (Exception ex)
                {
                    EventLogWriter.LogEvent(ex.Message, EventLogEntryType.Error, Event.FimExecucao);
                    GerenciadorException.GravarExcecao(ex);
                }
#if !DEBUG
                EventLogWriter.LogEvent(String.Format("Gerando notas para os pagamentos: {0}.", DateTime.Now), EventLogEntryType.Information, Event.InicioExecucao);
#endif
                try
                {
                    GerarNotas();
                }
                catch (Exception ex)
                {
                    EventLogWriter.LogEvent(ex.Message, EventLogEntryType.Error, Event.FimExecucao);
                    GerenciadorException.GravarExcecao(ex);
                }
#if !DEBUG
                EventLogWriter.LogEvent(String.Format("Terminou agora {0}.", DateTime.Now), EventLogEntryType.Information, Event.FimExecucao);    
                AjustarThread(DateTime.Now, false);
#endif
            }
        }
        #endregion

        #region GerarNotas
        /// <summary>
        ///     Gera as notas pendentes
        /// </summary>
        private void GerarNotas()
        {
            try
            {
                var lst = Pagamento.CarregarPagosSemNota();

#if !DEBUG
                EventLogWriter.LogEvent(String.Format("{0} notas para serem geradas.", lst.Count), EventLogEntryType.Information, Event.FimExecucao);
#endif

                var cont = 0;
                foreach (var objPagamento in lst)
                {
                    try
                    {
#if !DEBUG
                        if (cont % 10 == 0)
                            EventLogWriter.LogEvent(String.Format("{0}/{1} notas geradas.", cont, lst.Count), EventLogEntryType.Information, Event.FimExecucao);
#endif
                        // @TODO Armazenar no pagamento o usuario filial perfil que liberou o plano
                        PlanoParcela.EmitirNF(objPagamento, 3424172);

                        cont++;
                    }
                    catch (Exception ex)
                    {
                        EventLogWriter.LogEvent(ex.Message, EventLogEntryType.Error, Event.ErroExecucao);
                        GerenciadorException.GravarExcecao(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                EventLogWriter.LogEvent(ex.Message, EventLogEntryType.Error, Event.FimExecucao);
                GerenciadorException.GravarExcecao(ex);
            }
        }
        #endregion GerarNotas

        #region ObterNotas
        /// <summary>
        ///     Busca as notas ainda não importadas do plataforma para o BNE
        /// </summary>
        public void ObterNotas()
        {

            var lstNota = NotaFiscal.ObterNotas(Pagamento.CarregarPagamentosSemNF());

            foreach (var nota in lstNota)
            {
                Pagamento objPagamento;
                if (Pagamento.CarregarPagamentoPorNossoNumeroBoleto(nota.Transacao, out objPagamento))
                {
                    objPagamento.SalvarNotaFiscal(nota.NumeroNotaFiscal.ToString(), nota.Link);
                }
            }
        }
        #endregion ObterNotas

        #region AjustarThread
        private void AjustarThread(DateTime horaAtual, bool primeiraExecucao)
        {
            if (!primeiraExecucao)
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