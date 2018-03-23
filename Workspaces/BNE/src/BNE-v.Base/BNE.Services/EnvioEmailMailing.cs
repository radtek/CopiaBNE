using BNE.BLL;
using BNE.Services.Code;
using BNE.Services.Properties;
using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;

namespace BNE.Services
{
    partial class EnvioEmailMailing : ServiceBase
    {

        #region Propriedades
        private Thread _objThread;
        private static DateTime _dataHoraUltimaExecucao;
        private static readonly int DelayExecucao = Settings.Default.EnvioEmailMailingDelayMinutos;
        private const string EventSourceName = "EnvioEmailMailing";
        #endregion

        #region Construtores
        public EnvioEmailMailing()
        {
            InitializeComponent();
        }
        #endregion

        #region Eventos

        #region OnStart
        protected override void OnStart(string[] args)
        {
            //var objTs = new ThreadStart(IniciarEnvioEmail);
            var objTs = new ThreadStart(IniciarEnvioEmailNovo);
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

        #region IniciarEnvioEmailNovo
        public void IniciarEnvioEmailNovo()
        {
            try
            {
                AjustarThread(DateTime.Now);

                while (true)
                {
                    _dataHoraUltimaExecucao = DateTime.Now;
                    try
                    {
                        EventLogWriter.LogEvent(EventSourceName, String.Format("Iniciou agora {0}.", DateTime.Now), EventLogEntryType.Information, (int)EventID.InicioExecucao);

                        try
                        {
                            MensagemMailing.EnviarEmailsNaoEnviados(1000);
                        }
                        catch (Exception exEnvio)
                        {
                            string message;
                            var id = EL.GerenciadorException.GravarExcecao(exEnvio, out message);
                            message = string.Format("{0} - {1}", id, message);
                            EventLogWriter.LogEvent(EventSourceName, message, EventLogEntryType.Error, (int)EventID.ErroExecucao);
                        }
                    }
                    catch (Exception ex)
                    {
                        string message;
                        var id = EL.GerenciadorException.GravarExcecao(ex, out message);
                        message = string.Format("{0} - {1}", id, message);
                        EventLogWriter.LogEvent(EventSourceName, message, EventLogEntryType.Error, (int)EventID.ErroExecucao);
                    }

                    EventLogWriter.LogEvent(EventSourceName, String.Format("Terminou agora {0}.", DateTime.Now), EventLogEntryType.Information, (int)EventID.FimExecucao);
                    AjustarThread(DateTime.Now);
                }
            }
            catch (Exception ex)
            {
                string message;
                var id = EL.GerenciadorException.GravarExcecao(ex, out message);
                message = string.Format("{0} - {1}", id, message);
                EventLogWriter.LogEvent(EventSourceName, message, EventLogEntryType.Error, (int)EventID.ErroExecucao);
            }
        }
        #endregion

        #region AjustarThread
        private void AjustarThread(DateTime horaAtual)
        {
            TimeSpan tempoTotalExecucao = horaAtual - _dataHoraUltimaExecucao;
            var delay = new TimeSpan(0, DelayExecucao, 0);
            if ((int)delay.TotalMilliseconds - (int)tempoTotalExecucao.TotalMilliseconds > 0)
            {
                EventLogWriter.LogEvent(EventSourceName, String.Format("{2} - Está aguardando {0} para sua próxima execução, pois levou {1} para concluir a excução atual.", delay - tempoTotalExecucao, tempoTotalExecucao, EventSourceName), EventLogEntryType.Information, (int)EventID.AjusteExecucao);
                Thread.Sleep((int)delay.TotalMilliseconds - (int)tempoTotalExecucao.TotalMilliseconds);
            }
            else
                EventLogWriter.LogEvent(EventSourceName, String.Format("{4} - Vai reiniciar agora {0} pois sua última execução foi às {1}, seu tempo de execução foi de {2} e seu delay é de {3}.", DateTime.Now, _dataHoraUltimaExecucao, tempoTotalExecucao, (int)delay.TotalMilliseconds, EventSourceName), EventLogEntryType.Warning, (int)EventID.WarningAjusteExecucao);
        }
        #endregion

        #endregion

    }
}
