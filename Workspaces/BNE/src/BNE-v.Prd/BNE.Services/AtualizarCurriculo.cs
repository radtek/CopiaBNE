using System;
using System.Diagnostics;
using System.Threading;
using BNE.BLL;
using BNE.BLL.Common;
using BNE.BLL.Custom.Email;
using BNE.EL;
using BNE.Services.Base.EventLog;
using BNE.Services.Properties;
using LoginAutomatico = BNE.BLL.Custom.LoginAutomatico;

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
            {
                _objThread.Abort();
            }
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

                    if (DateTime.Today.DayOfWeek.Equals(DayOfWeek.Sunday))
                    {
                        EventLogWriter.LogEvent($"Iniciou agora {DateTime.Now}.", EventLogEntryType.Information, Event.InicioExecucao);

                        var curriculos = Curriculo.ListarCurriculosDesatualizados();

                        string assunto;
                        var templateMensagem = CartaEmail.RetornarConteudoBNE(BLL.Enumeradores.CartaEmail.AtualizarCurriculo, out assunto);
                        var emailRemetente = Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.EmailRemetenteAtualizarCV);

                        foreach (var curriculo in curriculos)
                        {
                            var parametros = new
                            {
                                NomeCandidato = curriculo.NomePessoa,
                                Link = LoginAutomatico.GerarHashAcessoLogin(curriculo.CPF, curriculo.DataNascimento, string.Empty)
                            };
                            var mensagem = FormatObject.ToString(parametros, templateMensagem);

                            var objUsuarioFilialPerfil = new UsuarioFilialPerfil(curriculo.IdUsuarioFilialPerfil);
                            var objCurriculo = new Curriculo(curriculo.Id);

                            EmailSenderFactory
                                .Create(TipoEnviadorEmail.Fila)
                                .Enviar(objCurriculo, null, objUsuarioFilialPerfil, assunto, mensagem, BLL.Enumeradores.CartaEmail.AtualizarCurriculo, emailRemetente, curriculo.Email);
                        }

                        EventLogWriter.LogEvent($"Terminou agora {DateTime.Now}.", EventLogEntryType.Information, Event.FimExecucao);
                    }
                    else
                    {
                        AjustarThread(DateTime.Now, false);
                    }
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
                {
                    horaParaExecucao = horaParaExecucao.AddDays(1);
                }

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
                {
                    EventLogWriter.LogEvent(
                        string.Format(
                            "Vai reiniciar agora {0} pois sua última execução foi às {1}, seu tempo de execução foi de {2} e seu delay é de {3}.",
                            DateTime.Now, _dataHoraUltimaExecucao, tempoTotalExecucao, (int) delay.TotalMilliseconds),
                        EventLogEntryType.Warning, Event.WarningAjusteExecucao);
                }
            }
        }

        #endregion

        #endregion
    }
}