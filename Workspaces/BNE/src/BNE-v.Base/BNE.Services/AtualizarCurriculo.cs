using System;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;
using BNE.Services.Properties;
using BNE.BLL;
using Enumeradores = BNE.BLL.Enumeradores;
using BNE.BLL.Custom;
using BNE.Services.Code;
using BNE.BLL.Custom.Email;

namespace BNE.Services
{
    partial class AtualizarCurriculo : ServiceBase
    {

        #region Propriedades
        private Thread _objThread;
        private static readonly string HoraExecucao = Settings.Default.AtualizarCurriculoHoraExecucao;
        private static readonly int DelayExecucao = Settings.Default.AtualizarCurriculoDelayMinutos;
        private const string EventSourceName = "AtualizarCurriculo";
        private static DateTime _dataHoraUltimaExecucao;
        #endregion

        #region Construtores
        public AtualizarCurriculo()
        {
            InitializeComponent();
        }
        #endregion

        #region Eventos

        #region OnStart
        protected override void OnStart(string[] args)
        {
            var objTs = new ThreadStart(IniciarAtualizarCurriculo);
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

        #region IniciarAtualizarCurriculo
        public void IniciarAtualizarCurriculo()
        {
            try
            {
                AjustarThread(DateTime.Now, true);
                
                while (true)
                {
                    _dataHoraUltimaExecucao = DateTime.Now;

                    EventLogWriter.LogEvent(EventSourceName, String.Format("Iniciou agora {0}.", DateTime.Now), EventLogEntryType.Information, (int)EventID.InicioExecucao);

                    // Envia para Currículos Desatualizados
                    DataTable dtCurriculo = Curriculo.ListarCurriculosDesatualizados();

                    if (dtCurriculo.Rows.Count > 0)
                        EnviarMensagemAtualizacaoCurriculo(dtCurriculo);

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

        #region EnviarMensagemAtualizacaoCurriculo
        private void EnviarMensagemAtualizacaoCurriculo(DataTable dtCurriculoEmail)
        {
            string assunto;
            string templateMensagem = CartaEmail.RetornarConteudoBNE(Enumeradores.CartaEmail.AtualizarCurriculo, out assunto);
            string emailRemetente = Parametro.RecuperaValorParametro(Enumeradores.Parametro.EmailMensagens);

            foreach (DataRow dr in dtCurriculoEmail.Rows)
            {
                var parametros = new
                    {
                        NomeCandidato = dr["Nme_Pessoa"].ToString()
                    };
                string mensagem = parametros.ToString(templateMensagem);

                var objUsuarioFilialPerfil = new UsuarioFilialPerfil(Convert.ToInt32(dr["Idf_Usuario_Filial_Perfil"]));
                var objCurriculo = new Curriculo(Convert.ToInt32(dr["Idf_Curriculo"]));

                EmailSenderFactory
                    .Create(TipoEnviadorEmail.Fila)
                    .Enviar(objCurriculo, null, objUsuarioFilialPerfil, assunto, mensagem, emailRemetente, dr["Eml_Pessoa"].ToString());
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
