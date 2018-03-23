using System;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;
using BNE.BLL;
using BNE.Services.Code;
using BNE.Services.Properties;
using Enumeradores = BNE.BLL.Enumeradores;
using BNE.BLL.Custom.Email;

namespace BNE.Services
{
    partial class AtualizarEmpresa : ServiceBase
    {

        #region Propriedades
        private Thread _objThread;
        private static readonly string HoraExecucao = Settings.Default.AtualizarEmpresaHoraExecucao;
        private static readonly int DelayExecucao = Settings.Default.AtualizarEmpresaDelayMinutos;
        private const string EventSourceName = "AtualizarEmpresa";
        private static DateTime _dataHoraUltimaExecucao;
        #endregion

        #region Construtores
        public AtualizarEmpresa()
        {
            InitializeComponent();
        }
        #endregion

        #region Eventos

        #region OnStart
        protected override void OnStart(string[] args)
        {
            var objTs = new ThreadStart(IniciarAtualizarEmpresa);
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

        #region Métodos

        #region IniciarAtualizarEmpresa
        public void IniciarAtualizarEmpresa()
        {
            try
            {

                AjustarThread(DateTime.Now, true);

                while (true)
                {
                    _dataHoraUltimaExecucao = DateTime.Now;

                    EventLogWriter.LogEvent(EventSourceName, String.Format("Iniciou agora {0}.", DateTime.Now), EventLogEntryType.Information, (int)EventID.InicioExecucao);
                    DataTable dtFiliais = Filial.ListaFiliaisDesatualizadas();

                    if (dtFiliais.Rows.Count > 0)
                        EnviarMensagemAtualizacaoEmpresa(dtFiliais);

                    EventLogWriter.LogEvent(EventSourceName, String.Format("Terminou agora {0}.", DateTime.Now), EventLogEntryType.Information, (int)EventID.FimExecucao);

                    AjustarThread(DateTime.Now, false);
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

        #region EnviarMensagemAtualizacaoEmpresa
        private void EnviarMensagemAtualizacaoEmpresa(DataTable dtFiliais)
        {
            string assunto;
            string template = CartaEmail.RetornarConteudoBNE(Enumeradores.CartaEmail.AtualizarEmpresa, out assunto);
            string emailRemetente = Parametro.RecuperaValorParametro(Enumeradores.Parametro.EmailMensagens);

            foreach (DataRow dr in dtFiliais.Rows)
            {
                EmailSenderFactory
                    .Create(TipoEnviadorEmail.Fila)
                    .Enviar(null, null, new UsuarioFilialPerfil(Convert.ToInt32(dr["Idf_Usuario_Filial_Perfil"])), assunto, template, emailRemetente, dr["Eml_Pessoa"].ToString());
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

                EventLogWriter.LogEvent(EventSourceName, String.Format("Está aguardando {0} para sua iniciar sua execução.", tempoParaExecutar.ToString()), EventLogEntryType.Information, (int)EventID.AjusteExecucao);
                Thread.Sleep((int)tempoParaExecutar.TotalMilliseconds);
            }
            else
            {
                TimeSpan tempoTotalExecucao = horaAtual - _dataHoraUltimaExecucao;
                var delay = new TimeSpan(0, DelayExecucao, 0);
                if ((int)delay.TotalMilliseconds - (int)tempoTotalExecucao.TotalMilliseconds > 0)
                {
                    EventLogWriter.LogEvent(EventSourceName, String.Format("Atualizar Empresa - Está aguardando {0} para sua próxima execução, pois levou {1} para concluir a excução atual.", delay - tempoTotalExecucao, tempoTotalExecucao), EventLogEntryType.Information, (int)EventID.AjusteExecucao);
                    Thread.Sleep((int)delay.TotalMilliseconds - (int)tempoTotalExecucao.TotalMilliseconds);
                }
                else
                    EventLogWriter.LogEvent(EventSourceName, String.Format("Atualizar Empresa - Vai reiniciar agora {0} pois sua última execução foi às {1}, seu tempo de execução foi de {2} e seu delay é de {3}.", DateTime.Now, _dataHoraUltimaExecucao, tempoTotalExecucao, (int)delay.TotalMilliseconds), EventLogEntryType.Warning, (int)EventID.WarningAjusteExecucao);
            }
        }
        #endregion

        #endregion

    }
}
