using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;
using BNE.BLL;
using BNE.BLL.Custom;
using BNE.Services.Code;
using BNE.Services.Properties;
using BNE.BLL.Custom.Email;


namespace BNE.Services
{
    partial class VagasSemCandidato : ServiceBase
    {
        #region Propriedades
        private Thread _objThread;
        private static DateTime _dataHoraUltimaExecucao;
        private static readonly int DelayExecucao = Settings.Default.VagasSemCandidatosDelayMinutos;
        private const string EventSourceName = "VagasSemCandidato";
        #endregion

        #region Construtores
        public VagasSemCandidato()
        {
            InitializeComponent();
        }
        #endregion

        #region Eventos

        #region OnStart

        protected override void OnStart(string[] args)
        {
            var objTs = new ThreadStart(IniciarVagasSemCandidto);
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

        #region IniciarVagasSemCandidato
        public void IniciarVagasSemCandidto() 
        {
            try {

                while (true) 
                {
                    _dataHoraUltimaExecucao = DateTime.Now;

                    AjustarThread(DateTime.Now, true);

                    try
                    {
                        EventLogWriter.LogEvent(EventSourceName, String.Format("Iniciou agora {0}.", DateTime.Now), EventLogEntryType.Information, (int)EventID.InicioExecucao);

                        DataTable dtVagas = BLL.Vaga.RecuperarVagasPoucosCandidatos();

                        if (dtVagas.Rows.Count > 0)
                            EnviarEmails(dtVagas);
                        else
                            return;
                    }
                    catch (Exception ex) 
                    {
                        string message;
                        var id = EL.GerenciadorException.GravarExcecao(ex, out message);
                        message = string.Format("{0} - {1}", id, message);
                        EventLogWriter.LogEvent(EventSourceName, message, EventLogEntryType.Error, (int)EventID.ErroExecucao);
                    }
                }
            }
            catch (Exception ex) {
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
                Settings.Default.Reload();

                TimeSpan InicioJanelaExecucao = Convert.ToDateTime(Settings.Default.VagasSemCandidatosHoraExecucao).TimeOfDay;
                TimeSpan TimeExecucao = DateTime.Now.TimeOfDay;

                if (InicioJanelaExecucao < TimeExecucao)
                {
                    return;
                }

                var horaParaExecucao = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, InicioJanelaExecucao.Hours, InicioJanelaExecucao.Minutes, 0);

                if (horaParaExecucao.Subtract(horaAtual).TotalMilliseconds < 0)
                    horaParaExecucao = horaParaExecucao.AddDays(1);

                TimeSpan tempoParaExecutar = horaParaExecucao - horaAtual;

                EventLogWriter.LogEvent(EventSourceName, String.Format("O Serviço está aguardando {0} para sua iniciar sua execução.", tempoParaExecutar.ToString()), EventLogEntryType.Information, (int)EventID.AjusteExecucao);
                Thread.Sleep((int)tempoParaExecutar.TotalMilliseconds);
            }
            else
            {
                var tempoTotalExecucao = horaAtual - _dataHoraUltimaExecucao;
                var delay = new TimeSpan(0, Settings.Default.VagasSemCandidatosDelayMinutos, 0);
                if ((int)delay.TotalMilliseconds - (int)tempoTotalExecucao.TotalMilliseconds > 0)
                {
                    EventLogWriter.LogEvent(EventSourceName, String.Format("O Serviço está aguardando {0} para sua próxima execução, pois levou {1} para concluir a excução atual.", delay - tempoTotalExecucao, tempoTotalExecucao), EventLogEntryType.Information, (int)EventID.AjusteExecucao);
                    Thread.Sleep((int)delay.TotalMilliseconds - (int)tempoTotalExecucao.TotalMilliseconds);
                }
                else
                    EventLogWriter.LogEvent(EventSourceName, String.Format("O Serviço vai reiniciar agora {0} pois sua última execução foi às {1}, seu tempo de execução foi de {2} e seu delay é de {3}.", DateTime.Now, _dataHoraUltimaExecucao, tempoTotalExecucao, (int)delay.TotalMilliseconds), EventLogEntryType.Warning, (int)EventID.WarningAjusteExecucao);
            }
        }
        #endregion

        #region EnviarEmails
        public void EnviarEmails(object parm) 
        {
            try
            {
                DataTable dt = (DataTable)parm;

                foreach (DataRow row in dt.Rows) 
                {
                    int idVaga = Convert.ToInt32(row["Idf_Vaga"]);
                    string emailVaga = row["Eml_Vaga"].ToString();
                    int idUsuarioFilialPerfil = Convert.ToInt32(row["Idf_Usuario_Filial_Perfil"]);

                    string assunto = string.Empty;
                    string mensagem = ConteudoHTML.RecuperaValorConteudo(BLL.Enumeradores.ConteudoHTML.VagasCadastrasSemCandidaturas);
                    string emailRemetente = Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.ContaPadraoEnvioEmail);

                    var objUsuarioFilialPerfil = UsuarioFilialPerfil.LoadObject(idUsuarioFilialPerfil);

                    EmailSenderFactory
                        .Create(TipoEnviadorEmail.Fila)
                        .Enviar(null, null, objUsuarioFilialPerfil, assunto, mensagem, emailRemetente, emailVaga);

                }

            }
            catch (Exception ex) 
            {
                EL.GerenciadorException.GravarExcecao(ex);
            }
        }
        #endregion

        #endregion

    }
}
