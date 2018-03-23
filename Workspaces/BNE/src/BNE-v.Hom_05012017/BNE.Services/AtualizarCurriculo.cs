using System;
using System.Data;
using System.Diagnostics;
using System.Threading;
using BNE.BLL;
using BNE.BLL.Custom;
using BNE.BLL.Custom.Email;
using BNE.EL;
using BNE.Services.Base.EventLog;
using BNE.Services.Properties;
using FormatObject = BNE.BLL.Common.FormatObject;

namespace BNE.Services
{
    internal partial class AtualizarCurriculo : BaseService
    {
        #region Construtores
        public AtualizarCurriculo()
        {
            InitializeComponent();
        }
        #endregion

        #region Propriedades
        private Thread _objThread;
        private static readonly string HoraExecucao = Settings.Default.AtualizarCurriculoHoraExecucao;
        private static readonly int DelayExecucao = Settings.Default.AtualizarCurriculoDelayMinutos;
        private static DateTime _dataHoraUltimaExecucao;
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

                    EventLogWriter.LogEvent(string.Format("Iniciou agora {0}.", DateTime.Now),
                        EventLogEntryType.Information, Event.InicioExecucao);

                    // Envia para Currículos Desatualizados
                    var dtCurriculo = Curriculo.ListarCurriculosDesatualizados();

                    if (dtCurriculo.Rows.Count > 0)
                        EnviarMensagemAtualizacaoCurriculo(dtCurriculo);

                    EventLogWriter.LogEvent(string.Format("Terminou agora {0}.", DateTime.Now),
                        EventLogEntryType.Information, Event.FimExecucao);

                    AjustarThread(DateTime.Now, false);
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

        #region EnviarMensagemAtualizacaoCurriculo
        private void EnviarMensagemAtualizacaoCurriculo(DataTable dtCurriculoEmail)
        {
            string assunto;
            var templateMensagem = CartaEmail.RetornarConteudoBNE(BLL.Enumeradores.CartaEmail.AtualizarCurriculo,
                out assunto);
            var emailRemetente = Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.EmailRemetenteAtualizarCV);
           
        
            foreach (DataRow dr in dtCurriculoEmail.Rows)
            {
                var parametros = new
                {
                    NomeCandidato = dr["Nme_Pessoa"].ToString(),
                    Link = LoginAutomatico.GerarHashAcessoLogin(Convert.ToDecimal(dr["Num_cpf"]), Convert.ToDateTime(dr["Dta_Nascimento"]), string.Empty)
                };
                var mensagem = FormatObject.ToString(parametros, templateMensagem);

                var objUsuarioFilialPerfil = new UsuarioFilialPerfil(Convert.ToInt32(dr["Idf_Usuario_Filial_Perfil"]));
                var objCurriculo = new Curriculo(Convert.ToInt32(dr["Idf_Curriculo"]));

                EmailSenderFactory
                    .Create(TipoEnviadorEmail.Fila)
                    .Enviar(objCurriculo, null, objUsuarioFilialPerfil, assunto, mensagem,BLL.Enumeradores.CartaEmail.AtualizarCurriculo, emailRemetente,
                        dr["Eml_Pessoa"].ToString());
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