using System;
using System.Diagnostics;
using System.Threading;
using BNE.BLL;
using BNE.EL;
using BNE.Services.Base.EventLog;
using BNE.Services.Properties;

namespace BNE.Services
{
    internal partial class EnviarCurriculo : BaseService
    {
        #region Construtor
        public EnviarCurriculo()
        {
            InitializeComponent();
            EventLogWriter = new EventLogWriter(Settings.Default.LogName, GetType().Name);
        }
        #endregion

        #region Propriedades
        private bool _primeiraExecucao;
        private Thread _objThread;
        private static DateTime _dataHoraUltimaExecucao;
        private static readonly string HoraExecucao = Settings.Default.EnviarCurriculoHoraExecucao;
        private static readonly int DelayExecucao = Settings.Default.EnviarCurriculoDelayMinutos;
        private EventLogWriter EventLogWriter { get; set; }
        #endregion

        #region Eventos

        #region OnStart
        protected override void OnStart(string[] args)
        {
            var objTs = new ThreadStart(IniciarEnviarCurriculo);
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

        #region IniciarEnviarCurriculo
        public void IniciarEnviarCurriculo()
        {
            try
            {
                _primeiraExecucao = true;

                while (true)
                {
                    _dataHoraUltimaExecucao = DateTime.Now;

                    EventLogWriter.LogEvent(string.Format("Iniciou agora {0}.", DateTime.Now), EventLogEntryType.Information, Event.InicioExecucao);
                    Vaga.NotificarCandidaturas();
                    EventLogWriter.LogEvent(string.Format("Terminou agora {0}.", DateTime.Now), EventLogEntryType.Information, Event.FimExecucao);

                    AjustarThread(DateTime.Now, _primeiraExecucao);
                    _primeiraExecucao = false;
                }
            }
            catch (Exception ex)
            {
                string message;
                var id = GerenciadorException.GravarExcecao(ex, out message);
                message = string.Format("{0} - {1}", id, message);
                EventLogWriter.LogEvent(message, EventLogEntryType.Error, Event.ErroExecucao);
            }
        }
        #endregion

        #region AjustarThread
        private void AjustarThread(DateTime horaAtual, bool primeiraExecucao)
        {
            if (primeiraExecucao)
            {
                var horaMinuto = HoraExecucao.Split(':');

                var horaParaExecucao = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                    Convert.ToInt32(horaMinuto[0]), Convert.ToInt32(horaMinuto[1]), 0);

                if (horaParaExecucao.Subtract(horaAtual).TotalMilliseconds < 0)
                    horaParaExecucao = horaParaExecucao.AddDays(1);

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