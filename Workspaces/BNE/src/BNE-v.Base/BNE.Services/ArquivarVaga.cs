using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using BNE.EL;
using System.Threading;
using BNE.Services.Properties;
using BNE.BLL;
using BNE.Services.Code;

namespace BNE.Services
{
    partial class ArquivarVaga : ServiceBase
    {
        #region Propriedades
        private Thread _objThread;
        private static readonly string _horaExecucao = Settings.Default.ArquivarVagaHoraExecucao;
        private static readonly int _delayExecucao = Settings.Default.ArquivarVagaDelayMinutos;
        private static DateTime _dataHoraUltimaExecucao;
        private const string EventSourceName = "ArquivarVaga";
        private bool primeiraExecucao;

        #endregion

        #region Contrutor
        public ArquivarVaga()
        {
            InitializeComponent();
        }
        #endregion

        #region Eventos

        #region OnStart

        protected override void OnStart(string[] args)
        {
            ThreadStart objTs = new ThreadStart(IniciarArquivarVaga);
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

        #region IniciarArquivarVaga
        public void IniciarArquivarVaga()
        {
            try
            {
                primeiraExecucao = true;

                while (true)
                {
                    _dataHoraUltimaExecucao = DateTime.Now;

                    EventLogWriter.LogEvent(EventSourceName, String.Format("Iniciou agora {0}.", DateTime.Now), EventLogEntryType.Information, (int)EventID.InicioExecucao);
                    Vaga.ArquivarVagas();
                    EventLogWriter.LogEvent(EventSourceName, String.Format("Terminou agora {0}.", DateTime.Now), EventLogEntryType.Information, (int)EventID.FimExecucao);
                    
                    AjustarThread(DateTime.Now, primeiraExecucao);
                    primeiraExecucao = false;
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
        private void AjustarThread(DateTime horaAtual, bool primeiraExecucao)
        {
            if (primeiraExecucao)
            {
                string[] horaMinuto = _horaExecucao.Split(':');

                DateTime horaParaExecucao = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Convert.ToInt32(horaMinuto[0]), Convert.ToInt32(horaMinuto[1]), 0);

                if (horaParaExecucao.Subtract(horaAtual).TotalMilliseconds < 0)
                    horaParaExecucao = horaParaExecucao.AddDays(1);

                TimeSpan tempoParaExecutar = horaParaExecucao - horaAtual;

                EventLogWriter.LogEvent(EventSourceName, String.Format("Está aguardando {0} para sua iniciar sua execução.", tempoParaExecutar.ToString()), EventLogEntryType.Information, (int)EventID.AjusteExecucao);
                Thread.Sleep((int)tempoParaExecutar.TotalMilliseconds);
            }
            else
            {
                TimeSpan tempoTotalExecucao = horaAtual - _dataHoraUltimaExecucao;
                TimeSpan delay = new TimeSpan(0, _delayExecucao, 0);
                if ((int)delay.TotalMilliseconds - (int)tempoTotalExecucao.TotalMilliseconds > 0)
                {
                    EventLogWriter.LogEvent(EventSourceName, String.Format("Está aguardando {0} para sua próxima execução, pois levou {1} para concluir a excução atual.", delay - tempoTotalExecucao, tempoTotalExecucao), EventLogEntryType.Information, (int)EventID.AjusteExecucao);
                    Thread.Sleep((int)delay.TotalMilliseconds - (int)tempoTotalExecucao.TotalMilliseconds);
                }
                else
                    EventLogWriter.LogEvent(EventSourceName, String.Format("Vai reiniciar agora {0} pois sua última execução foi às {1}, seu tempo de execução foi de {2} e seu delay é de {3}.", DateTime.Now, _dataHoraUltimaExecucao, tempoTotalExecucao, (int)delay.TotalMilliseconds), EventLogEntryType.Warning, (int)EventID.WarningAjusteExecucao);
            }
        }
        #endregion

        #endregion
    }
}
