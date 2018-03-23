using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Threading;
using BNE.BLL;
using BNE.BLL.Integracoes;
using BNE.BLL.Integracoes.SendGrid;
using BNE.EL;
using BNE.Services.Base.EventLog;
using BNE.Services.Properties;
using Parametro = BNE.BLL.Enumeradores.Parametro;

namespace BNE.Services
{
    public partial class EmailsInvalidos : BaseService
    {
        #region Construtor
        public EmailsInvalidos()
        {
            InitializeComponent();
        }
        #endregion

        #region Propriedades
        private Thread _objThread;
        private static DateTime _dataHoraUltimaExecucao;
        private static readonly string HoraExecucao = Settings.Default.EmailsInvalidosHoraExecucao;
        private static readonly int DelayExecucao = Settings.Default.EmailsInvalidosDelayMinutos;
        private const int TAMANHOLOTE = 75;
        #endregion

        #region Eventos

        #region OnStart
        protected override void OnStart(string[] args)
        {
            var objTs = new ThreadStart(Iniciar);
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
                    string.Format(
                        "Serviço Emails Inválidos - O Serviço está aguardando {0} para sua iniciar sua execução.",
                        tempoParaExecutar), EventLogEntryType.Information, Event.AjusteExecucao);
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
                            "Serviço Emails Inválidos - O Serviço está aguardando {0} para sua próxima execução, pois levou {1} para concluir a execução atual.",
                            delay - tempoTotalExecucao, tempoTotalExecucao), EventLogEntryType.Information,
                        Event.AjusteExecucao);
                    Thread.Sleep((int) delay.TotalMilliseconds - (int) tempoTotalExecucao.TotalMilliseconds);
                }
                else
                    EventLogWriter.LogEvent(
                        string.Format(
                            "Serviço Emails Inválidos - O Serviço vai reiniciar agora {0} pois sua última execução foi às {1}, seu tempo de execução foi de {2} e seu delay é de {3}.",
                            DateTime.Now, _dataHoraUltimaExecucao, tempoTotalExecucao, (int) delay.TotalMilliseconds),
                        EventLogEntryType.Warning, Event.WarningAjusteExecucao);
            }
        }
        #endregion

        #region Iniciar
        public void Iniciar()
        {
            try
            {
                AjustarThread(DateTime.Now, true);
                while (true)
                {
                    _dataHoraUltimaExecucao = DateTime.Now;

                    EventLogWriter.LogEvent(string.Format("Iniciou agora {0}.", DateTime.Now),
                        EventLogEntryType.Information, Event.InicioExecucao);

                    AtualizarStatusEnderecosEmail(); // executa operação

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

        #region AtualizarStatusEnderecosEmail
        public void AtualizarStatusEnderecosEmail()
        {
            if (VerificarDataImportacao(Parametro.DataImportacaoEmailsInvalidos))
            {
                AtualizarStatusEnderecosEmailSendGrid();

                // persiste nova data de importação (vai alterar o startDate acima)
                PersistirNovaDataImportacao(Parametro.DataImportacaoEmailsInvalidos);
            }
            else
                EventLogWriter.LogEvent(
                    "Serviço de Emails inválidos - Data de importação inválida; veja tabela Parametro",
                    EventLogEntryType.Error, Event.ErroExecucao);
        }
        #endregion

        #region VerificarDataImportacao
        private bool VerificarDataImportacao(Parametro parametro)
        {
            var parametroStartDate = BLL.Parametro.RecuperaValorParametro(parametro);

            DateTime startDate;
            if (
                !DateTime.TryParse(parametroStartDate, CultureInfo.GetCultureInfo("pt-br"), DateTimeStyles.AssumeLocal,
                    out startDate))
            {
                throw new InvalidOperationException(
                    "A data de importação de emails inválidos está em um formato incorreto: '" + parametroStartDate +
                    "', esperado: dd/MM/yyyy");
            }

            return (DateTime.Now - startDate).TotalDays > 0d; // se startDate for hoje ou antes, retorna true
        }
        #endregion

        #region AtualizarStatusEnderecosEmailSendGrid
        public void AtualizarStatusEnderecosEmailSendGrid()
        {
            var startDate =
                BLL.Parametro.RecuperaValorParametro(Parametro.DataImportacaoEmailsInvalidos);
            var apiUser =
                BLL.Parametro.RecuperaValorParametro(Parametro.SendGridApiUser);
            var apiKey =
                BLL.Parametro.RecuperaValorParametro(Parametro.SendGridApiKey);

            var client =
                new HttpClientSendGridApi(apiUser, apiKey, startDate);

            // não alterar a ordem abaixo
            AtualizarSendGridBlocked(client);
            AtualizarSendGridBounced(client);
            AtualizarSendGridInvalid(client);
            AtualizarSendGridUnsubscribe(client);
        }
        #endregion

        #region Atualizacao
        /// <summary>
        ///     Atualiza de acordo com o enumerador informado e o BLO de persistência informado
        /// </summary>
        /// <param name="enumerador">Enumerador de status de email, para gerar uma string com emails separados pelo texto " or "</param>
        /// <param name="persistencia">BLO de persistencia de emails usando a string gerada a partir do enumerador</param>
        private void Atualizacao(Func<IEnumerable<IEmailStatus>> enumerador, Action<string> persistencia)
        {
            var i = 0;
            var sb = new StringBuilder();

            foreach (var status in enumerador())
            {
                ++i;

                if (i > 1)
                    sb.Append(" or ");

                var email = status.Email;
                sb.Append(email);

                if (i >= TAMANHOLOTE)
                {
                    persistencia(sb.ToString());
                    sb.Clear();
                    i = 0;
                }
            }

            if (i > 0)
                persistencia(sb.ToString());
        }
        #endregion

        #region AtualizarSendGridBlocked
        private void AtualizarSendGridBlocked(HttpClientSendGridApi client)
        {
            Atualizacao(() => client.GetBlockedEmails(), lista => PessoaFisica.AtualizarEmailBloqueado(lista));
        }
        #endregion

        #region AtualizarSendGridBounced
        private void AtualizarSendGridBounced(HttpClientSendGridApi client)
        {
            Atualizacao(() => client.GetBouncedEmails(), lista => PessoaFisica.AtualizarEmailBounce(lista));
        }
        #endregion

        #region AtualizarSendGridInvalid
        private void AtualizarSendGridInvalid(HttpClientSendGridApi client)
        {
            Atualizacao(() => client.GetInvalidEmails(), lista => PessoaFisica.AtualizarEmailInvalido(lista));
        }
        #endregion

        #region AtualizarSendGridUnsubscribe
        private void AtualizarSendGridUnsubscribe(HttpClientSendGridApi client)
        {
            Atualizacao(() => client.GetUnsubscribeEmails(), lista => PessoaFisica.AtualizarEmailUnsubscribe(lista));
        }
        #endregion

        #region PersistirNovaDataImportacao
        private void PersistirNovaDataImportacao(Parametro parametro)
        {
            var dict = BLL.Parametro.ListarParametros(
                new List<Parametro> {parametro});

            var parametroStartDate = dict[parametro];

            DateTime startDate;
            if (
                !DateTime.TryParse(parametroStartDate, CultureInfo.GetCultureInfo("pt-br"), DateTimeStyles.AssumeLocal,
                    out startDate))
            {
                throw new InvalidOperationException(
                    "A data de importação de emails inválidos está em um formato incorreto: '" + parametroStartDate +
                    "', esperado: dd/MM/yyyy");
            }

            if (startDate <= DateTime.Now)
            {
                var endDate = startDate.AddDays(1);

                dict[parametro] = endDate.ToString("dd/MM/yyyy");

                BLL.Parametro.SalvarParametros(dict);
            }
        }
        #endregion

        #endregion
    }
}