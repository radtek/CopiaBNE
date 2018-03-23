using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;
using BNE.BLL;
using BNE.EL;
using BNE.Services.Code;
using BNE.Services.Properties;

namespace BNE.Services
{
    partial class EmailVagaFacil : ServiceBase
    {

        #region Propriedades
        private Thread _objThread;
        private static DateTime _dataHoraUltimaExecucaoEnvio;
        private static DateTime _dataHoraUltimaExecucaoRetorno;
        private static readonly int DelayExecucaoEnvio = Settings.Default.EmailVagaFacilDelayMinutosEnvio;
        private static readonly string EventSourceNameEnvio = Settings.Default.EmailVagaFacilDisplayNameEnvio;
        private static readonly int DelayExecucaoRetorno = Settings.Default.EmailVagaFacilDelayMinutosRetorno;
        private static readonly string EventSourceNameRetorno = Settings.Default.EmailVagaFacilDisplayNameRetorno;
        #endregion

        #region Contrutor
        public EmailVagaFacil()
        {
            InitializeComponent();
        }
        #endregion

        #region Eventos

        #region OnStart
        protected override void OnStart(string[] args)
        {
            var objTs = new ThreadStart(IniciarEnvioVagaFacil);
            _objThread = new Thread(objTs);
            _objThread.Start();

            objTs = new ThreadStart(IniciarRetornoEnvioVagaFacil);
            _objThread = new Thread(objTs);
            _objThread.Start();
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

        #region IniciarEnvioVagaFacil
        public void IniciarEnvioVagaFacil()
        {
            try
            {
                EventLogWriter.AjustarEventSource(EventSourceNameEnvio);

                while (true)
                {
                    _dataHoraUltimaExecucaoEnvio = DateTime.Now;

                    EventLogWriter.LogEvent(EventSourceNameEnvio, String.Format("Iniciou agora {0}.", DateTime.Now), EventLogEntryType.Information, (int)EventID.InicioExecucao);

                    Vaga.RoboEmailVagaFacil();

                    EventLogWriter.LogEvent(EventSourceNameEnvio, String.Format("Terminou agora {0}.", DateTime.Now), EventLogEntryType.Information, (int)EventID.FimExecucao);

                    AjustarThread(DateTime.Now, DelayExecucaoEnvio, EventSourceNameEnvio, _dataHoraUltimaExecucaoEnvio);
                }
            }
            catch (Exception ex)
            {
                Guid id = EL.GerenciadorException.GravarExcecao(ex);
                EventLogWriter.LogEvent(EventSourceNameEnvio, id.ToString(), EventLogEntryType.Error, (int)EventID.ErroExecucao);
            }
        }
        #endregion

        #region IniciarRetornoEnvioVagaFacil
        public void IniciarRetornoEnvioVagaFacil()
        {
            try
            {
                EventLogWriter.AjustarEventSource(EventSourceNameRetorno);

                while (true)
                {
                    _dataHoraUltimaExecucaoRetorno = DateTime.Now;

                    EventLogWriter.LogEvent(EventSourceNameRetorno, String.Format("Iniciou agora {0}.", DateTime.Now), EventLogEntryType.Information, (int)EventID.InicioExecucao);

                    Vaga.RoboRetornoEmailVagaFacil();

                    EventLogWriter.LogEvent(EventSourceNameRetorno, String.Format("Terminou agora {0}.", DateTime.Now), EventLogEntryType.Information, (int)EventID.FimExecucao);

                    AjustarThread(DateTime.Now, DelayExecucaoRetorno, EventSourceNameRetorno, _dataHoraUltimaExecucaoRetorno);
                }
            }
            catch (Exception ex)
            {
                string message;
                Guid id = EL.GerenciadorException.GravarExcecao(ex, out message);
                message = string.Format("{0} - {1}", id, message);
                EventLogWriter.LogEvent(EventSourceNameRetorno, message, EventLogEntryType.Error, (int)EventID.ErroExecucao);
            }
        }
        #endregion

        #region AjustarThread
        private void AjustarThread(DateTime horaFinal, int delayExecucao, string eventSourceName, DateTime dataHoraUltimaExecucao)
        {
            var tempoTotalExecucao = horaFinal - dataHoraUltimaExecucao;
            var delay = new TimeSpan(0, delayExecucao, 0);
            if ((int)delay.TotalMilliseconds - (int)tempoTotalExecucao.TotalMilliseconds > 0)
            {
                EventLogWriter.LogEvent(eventSourceName, String.Format("Está aguardando {0} para sua próxima execução, pois levou {1} para concluir a excução atual.", delay - tempoTotalExecucao, tempoTotalExecucao), EventLogEntryType.Information, (int)EventID.AjusteExecucao);
                Thread.Sleep((int)delay.TotalMilliseconds - (int)tempoTotalExecucao.TotalMilliseconds);
            }
            else
                EventLogWriter.LogEvent(eventSourceName, String.Format("Vai reiniciar agora {0} pois sua última execução foi às {1}, seu tempo de execução foi de {2} e seu delay é de {3}.", DateTime.Now, dataHoraUltimaExecucao, tempoTotalExecucao, (int)delay.TotalMilliseconds), EventLogEntryType.Warning, (int)EventID.WarningAjusteExecucao);
        }
        #endregion

        #endregion

    }
}
