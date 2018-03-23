using System;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Threading;
using System.Globalization;
using System.Collections.Generic;
using BNE.Services.Code;
using BNE.Services.Properties;
using BNE.BLL;
using Enumeradores = BNE.BLL.Enumeradores;
using BNE.BLL.Integracoes.SendGrid;
using BNE.BLL.Integracoes.Mandrill;
using System.Text;

namespace BNE.Services
{
    public partial class EmailsInvalidos : ServiceBase
    {
        #region Propriedades
        private Thread _objThread;
        private static DateTime _dataHoraUltimaExecucao;
        private static readonly string HoraExecucao = Settings.Default.EmailsInvalidosHoraExecucao;
        private static readonly int DelayExecucao = Settings.Default.EmailsInvalidosDelayMinutos;
        private const string EventSourceName = "EmailsInvalidos";
        private const int TAMANHOLOTE = 75;
        #endregion

        #region Construtor
        public EmailsInvalidos()
        {
            InitializeComponent();
        }
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
                string[] horaMinuto = HoraExecucao.Split(':');

                var horaParaExecucao = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Convert.ToInt32(horaMinuto[0]), Convert.ToInt32(horaMinuto[1]), 0);

                if (horaParaExecucao.Subtract(horaAtual).TotalMilliseconds < 0)
                    horaParaExecucao = horaParaExecucao.AddDays(1);

                TimeSpan tempoParaExecutar = horaParaExecucao - horaAtual;

                EventLogWriter.LogEvent(EventSourceName, String.Format("Serviço Emails Inválidos - O Serviço está aguardando {0} para sua iniciar sua execução.", tempoParaExecutar.ToString()), EventLogEntryType.Information, (int)EventID.AjusteExecucao);
                Thread.Sleep((int)tempoParaExecutar.TotalMilliseconds);
            }
            else
            {
                var tempoTotalExecucao = horaAtual - _dataHoraUltimaExecucao;
                var delay = new TimeSpan(0, DelayExecucao, 0);
                if ((int)delay.TotalMilliseconds - (int)tempoTotalExecucao.TotalMilliseconds > 0)
                {
                    EventLogWriter.LogEvent(EventSourceName, String.Format("Serviço Emails Inválidos - O Serviço está aguardando {0} para sua próxima execução, pois levou {1} para concluir a execução atual.", delay - tempoTotalExecucao, tempoTotalExecucao), EventLogEntryType.Information, (int)EventID.AjusteExecucao);
                    Thread.Sleep((int)delay.TotalMilliseconds - (int)tempoTotalExecucao.TotalMilliseconds);
                }
                else
                    EventLogWriter.LogEvent(EventSourceName, String.Format("Serviço Emails Inválidos - O Serviço vai reiniciar agora {0} pois sua última execução foi às {1}, seu tempo de execução foi de {2} e seu delay é de {3}.", DateTime.Now, _dataHoraUltimaExecucao, tempoTotalExecucao, (int)delay.TotalMilliseconds), EventLogEntryType.Warning, (int)EventID.WarningAjusteExecucao);
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

                    EventLogWriter.LogEvent(EventSourceName, String.Format("Iniciou agora {0}.", DateTime.Now), EventLogEntryType.Information, (int)EventID.InicioExecucao);

                    AtualizarStatusEnderecosEmail();    // executa operação

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

        #region AtualizarStatusEnderecosEmail
        public void AtualizarStatusEnderecosEmail()
        {
            if (VerificarDataImportacao(Enumeradores.Parametro.DataImportacaoEmailsInvalidos))
            {
                AtualizarStatusEnderecosEmailSendGrid();
                AtualizarStatusEnderecosEmailMandrill();

                // persiste nova data de importação (vai alterar o startDate acima)
                PersistirNovaDataImportacao(Enumeradores.Parametro.DataImportacaoEmailsInvalidos);
            }
            else
                EventLogWriter.LogEvent(EventSourceName, "Serviço de Emails inválidos - Data de importação inválida; veja tabela Parametro", EventLogEntryType.Error, (int)EventID.ErroExecucao);
        }
        #endregion

        #region VerificarDataImportacao
        private bool VerificarDataImportacao(Enumeradores.Parametro parametro)
        {
            string parametroStartDate = Parametro.RecuperaValorParametro(parametro);

            DateTime startDate;
            if (!DateTime.TryParse(parametroStartDate, CultureInfo.GetCultureInfo("pt-br"), DateTimeStyles.AssumeLocal, out startDate))
            {
                throw new InvalidOperationException("A data de importação de emails inválidos está em um formato incorreto: '" + parametroStartDate + "', esperado: dd/MM/yyyy");
            }

            return (DateTime.Now - startDate).TotalDays > 0d; // se startDate for hoje ou antes, retorna true
        }
        #endregion

        #region AtualizarStatusEnderecosEmailSendGrid
        public void AtualizarStatusEnderecosEmailSendGrid()
        {
            string startDate =
                BLL.Parametro.RecuperaValorParametro(Enumeradores.Parametro.DataImportacaoEmailsInvalidos);
            string apiUser =
                BLL.Parametro.RecuperaValorParametro(Enumeradores.Parametro.SendGridApiUser);
            string apiKey =
                BLL.Parametro.RecuperaValorParametro(Enumeradores.Parametro.SendGridApiKey);

            HttpClientSendGridApi client = 
                new HttpClientSendGridApi(apiUser, apiKey, startDate);
            
            // não alterar a ordem abaixo
            AtualizarSendGridBlocked(client);
            AtualizarSendGridBounced(client);
            AtualizarSendGridInvalid(client);
            AtualizarSendGridUnsubscribe(client);
        }
        #endregion

        #region AtualizarStatusEnderecosEmailMandrill
        public void AtualizarStatusEnderecosEmailMandrill()
        {
            string startDate =
                BLL.Parametro.RecuperaValorParametro(Enumeradores.Parametro.DataImportacaoEmailsInvalidos);
            string apiKey =
                BLL.Parametro.RecuperaValorParametro(Enumeradores.Parametro.MandrillApiKey);

            HttpClientMandrillApi client =
                new HttpClientMandrillApi(apiKey, startDate);

            // não alterar a ordem abaixo
            AtualizarMandrillSpammed(client);
            AtualizarMandrillBounced(client);
            AtualizarMandrillUnsubscribe(client);
        }
        #endregion

        #region Atualizacao
        /// <summary>
        /// Atualiza de acordo com o enumerador informado e o BLO de persistência informado
        /// </summary>
        /// <param name="enumerador">Enumerador de status de email, para gerar uma string com emails separados pelo texto " or "</param>
        /// <param name="persistencia">BLO de persistencia de emails usando a string gerada a partir do enumerador</param>
        private void Atualizacao(Func<IEnumerable<BLL.Integracoes.IEmailStatus>> enumerador, Action<string> persistencia)
        {
            int i = 0;
            StringBuilder sb = new StringBuilder();

            foreach (var status in enumerador())
            {
                ++i;

                if (i > 1)
                    sb.Append(" or ");

                string email = status.Email;
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
            Atualizacao(() => client.GetBlockedEmails(), lista => BLL.PessoaFisica.AtualizarEmailBloqueado(lista));
        }
        #endregion

        #region AtualizarSendGridBounced
        private void AtualizarSendGridBounced(HttpClientSendGridApi client)
        {
            Atualizacao(() => client.GetBouncedEmails(), lista => BLL.PessoaFisica.AtualizarEmailBounce(lista));
        }
        #endregion

        #region AtualizarSendGridInvalid
        private void AtualizarSendGridInvalid(HttpClientSendGridApi client)
        {
            Atualizacao(() => client.GetInvalidEmails(), lista => BLL.PessoaFisica.AtualizarEmailInvalido(lista));
        }
        #endregion

        #region AtualizarSendGridUnsubscribe
        private void AtualizarSendGridUnsubscribe(HttpClientSendGridApi client)
        {
            Atualizacao(() => client.GetUnsubscribeEmails(), lista => BLL.PessoaFisica.AtualizarEmailUnsubscribe(lista));
        }
        #endregion

        #region AtualizarMandrillSpammed
        private void AtualizarMandrillSpammed(HttpClientMandrillApi client)
        {
            Atualizacao(() => client.GetSpammedEmails(), lista => BLL.PessoaFisica.AtualizarEmailBloqueado(lista));
        }
        #endregion

        #region AtualizarMandrillBounce
        private void AtualizarMandrillBounced(HttpClientMandrillApi client)
        {
            Atualizacao(() => client.GetBounceEmails(), lista => BLL.PessoaFisica.AtualizarEmailBounce(lista));
        }
        #endregion

        #region AtualizarMandrillUnsubscribe
        private void AtualizarMandrillUnsubscribe(HttpClientMandrillApi client)
        {
            Atualizacao(() => client.GetUnsubscribeEmails(), lista => BLL.PessoaFisica.AtualizarEmailUnsubscribe(lista));
        }
        #endregion

        #region PersistirNovaDataImportacao
        private void PersistirNovaDataImportacao(Enumeradores.Parametro parametro)
        {
            var dict = Parametro.ListarParametros(
                new List<Enumeradores.Parametro>() { parametro });

            string parametroStartDate = dict[parametro];

            DateTime startDate;
            if (!DateTime.TryParse(parametroStartDate, CultureInfo.GetCultureInfo("pt-br"), DateTimeStyles.AssumeLocal, out startDate))
            {
                throw new InvalidOperationException("A data de importação de emails inválidos está em um formato incorreto: '" + parametroStartDate + "', esperado: dd/MM/yyyy");
            }

            if (startDate <= DateTime.Now)
            {
                DateTime endDate = startDate.AddDays(1);

                dict[parametro] = endDate.ToString("dd/MM/yyyy");

                Parametro.SalvarParametros(dict);
            }
        }
        #endregion

        #endregion
    }
}
