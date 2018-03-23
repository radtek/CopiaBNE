using System.Data;
using System.Diagnostics;
using System.Text;
using System.Threading;
using BNE.BLL;
using BNE.BLL.Custom;
using BNE.BLL.Custom.Email;
using BNE.BLL.Enumeradores;
using BNE.EL;
using BNE.Services.Base.EventLog;
using BNE.Services.Properties;
using CartaEmail = BNE.BLL.CartaEmail;
using Parametro = BNE.BLL.Parametro;
using System;

namespace BNE.Services
{
    partial class EnvioEmailParaSMSNaoRecebida : BaseService
    {
        #region Construtores
        public EnvioEmailParaSMSNaoRecebida()
        {
            InitializeComponent();
        }
        #endregion

        #region Propriedades
        private Thread _objThread;
        private static readonly string HoraExecucao = Settings.Default.EnvioEmailParaSMSNaoRecebidaHoraExecucao;
        private static readonly int DelayExecucao = Settings.Default.EnvioEmailParaSMSNaoRecebidaDelay;
        private DateTime _dataHoraUltimaExecucao;
        #endregion

        #region Eventos
        protected override void OnStart(string[] args)
        {
            var objTs = new ThreadStart(IniciarEnvio);
            _objThread = new Thread(objTs);
            _objThread.Start();
        }

        protected override void OnStop()
        {
            if (_objThread != null)
                _objThread.Abort();
        }
        #endregion

        #region Métodos

        #region IniciarEnvio
        public void IniciarEnvio()
        {
            try
            {
               // AjustarThread(DateTime.Now, true);

                while (true)
                {
                    _dataHoraUltimaExecucao = DateTime.Now;

                    try
                    {
#if !DEBUG
                            EventLogWriter.LogEvent(String.Format("Iniciou agora o envio da carta para candidatos que não teve retorno as sms do dia {0}.", DateTime.Now), 
                            EventLogEntryType.Information, Event.InicioExecucao);
#endif

                        //Pegar a lista de pessoas que não receberam sms, regra de com 3 tentativas falhada.

                           BLL.CampanhaMensagemEnvios.CandidatosSemRetorno();
                         

                    }
                    catch (Exception exEnvio)
                    {
#if !DEBUG
                        string message;
                        var id = EL.GerenciadorException.GravarExcecao(exEnvio, out message);
                        message = string.Format("{0} - {1}", id, message);
                        
                                EventLogWriter.LogEvent(message, EventLogEntryType.Error, Event.ErroExecucao);
                        
#endif
                    }
#if !DEBUG
                      EventLogWriter.LogEvent(String.Format("Terminou agora {0}.", DateTime.Now), EventLogEntryType.Information, Event.FimExecucao);
                      AjustarThread(DateTime.Now, false);
#endif
                }
            }
            catch (Exception ex)
            {
                string message;
                var id = GerenciadorException.GravarExcecao(ex, out message);
                message = string.Format("{0} - {1}", id, message);
#if !DEBUG
                    EventLogWriter.LogEvent(message, EventLogEntryType.Error, Event.ErroExecucao);     
#endif
            }
        }
        #endregion

        #region AjustarThread
        private void AjustarThread(DateTime horaAtual, bool primeiraExecucao)
        {

            if (primeiraExecucao)
            {
                string[] horaMinuto = HoraExecucao.Split(':');

                var horaParaExecucao = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Convert.ToInt32(horaMinuto[0]), Convert.ToInt32(horaMinuto[1]), 0);

                if (horaParaExecucao.Subtract(horaAtual).TotalMilliseconds < 0)
                    horaParaExecucao = horaParaExecucao.AddDays(1);

                TimeSpan tempoParaExecutar = horaParaExecucao - horaAtual;

                EventLogWriter.LogEvent(String.Format("Está aguardando {0} para sua iniciar sua execução.", tempoParaExecutar.ToString()), EventLogEntryType.Information, Event.AjusteExecucao);
                Thread.Sleep((int)tempoParaExecutar.TotalMilliseconds);
            }
            else
            {
                TimeSpan tempoTotalExecucao = horaAtual - _dataHoraUltimaExecucao;
                var delay = new TimeSpan(0, DelayExecucao, 0);
                if ((int)delay.TotalMilliseconds - (int)tempoTotalExecucao.TotalMilliseconds > 0)
                {
                    EventLogWriter.LogEvent(String.Format("Está aguardando {0} para sua próxima execução, pois levou {1} para concluir a excução atual.", delay - tempoTotalExecucao, tempoTotalExecucao), EventLogEntryType.Information, Event.AjusteExecucao);
                    Thread.Sleep((int)delay.TotalMilliseconds - (int)tempoTotalExecucao.TotalMilliseconds);
                }
                else
                    EventLogWriter.LogEvent(String.Format("Vai reiniciar agora {0} pois sua última execução foi às {1}, seu tempo de execução foi de {2} e seu delay é de {3}.", DateTime.Now, _dataHoraUltimaExecucao, tempoTotalExecucao, (int)delay.TotalMilliseconds), EventLogEntryType.Warning, Event.WarningAjusteExecucao);
            }

        }
        #endregion

        #endregion
    }
}