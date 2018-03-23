using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Threading;
using BNE.BLL;
using BNE.BLL.DTO;
using BNE.EL;
using BNE.Services.Base.EventLog;
using BNE.Services.Properties;
using Curriculo = BNE.BLL.Curriculo;

namespace BNE.Services
{
    internal partial class EnvioSMSSemanal : BaseService
    {
        #region Construtores
        public EnvioSMSSemanal()
        {
            InitializeComponent();
        }
        #endregion

        #region Propriedades
        private Thread _objThread;
        private static readonly string HoraExecucao = Settings.Default.EnvioSMSSemanalHoraExecucao;
        private static readonly int DelayExecucao = Settings.Default.EnvioSMSSemanalDelayMinutos;
        private static DateTime _dataHoraUltimaExecucao;
        #endregion

        #region Eventos

        #region OnStart
        protected override void OnStart(string[] args)
        {
            var objTs = new ThreadStart(IniciarEnvioSMSSemanal);
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

        #region IniciarEnvioSMSSemanal
        public void IniciarEnvioSMSSemanal()
        {
            try
            {
                AjustarThread(DateTime.Now, true);

                while (true)
                {
                    _dataHoraUltimaExecucao = DateTime.Now;

                    if (DateTime.Today.DayOfWeek.Equals(DayOfWeek.Thursday))
                    {
                        EventLogWriter.LogEvent(string.Format("Iniciou agora {0}.", DateTime.Now),
                            EventLogEntryType.Information, Event.InicioExecucao);

                        InicializarEnvioSMSSemanal();
                        InicializarEnvioSMSSemanalNaoCandidatou();

                        EventLogWriter.LogEvent(string.Format("Terminou agora {0}.", DateTime.Now),
                            EventLogEntryType.Information, Event.FimExecucao);

                        AjustarThread(DateTime.Now, false);
                    }
                    else
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
                    string.Format("Envio SMS Semanal - Está aguardando {0} para sua iniciar sua execução.",
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
                            "Envio SMS Semanal - Está aguardando {0} para sua próxima execução, pois levou {1} para concluir a excução atual.",
                            delay - tempoTotalExecucao, tempoTotalExecucao), EventLogEntryType.Information,
                        Event.AjusteExecucao);
                    Thread.Sleep((int) delay.TotalMilliseconds - (int) tempoTotalExecucao.TotalMilliseconds);
                }
                else
                    EventLogWriter.LogEvent(
                        string.Format(
                            "Envio SMS Semanal - Vai reiniciar agora {0} pois sua última execução foi às {1}, seu tempo de execução foi de {2} e seu delay é de {3}.",
                            DateTime.Now, _dataHoraUltimaExecucao, tempoTotalExecucao, (int) delay.TotalMilliseconds),
                        EventLogEntryType.Warning, Event.WarningAjusteExecucao);
            }
        }
        #endregion

        #region InicializarEnvioSMSSemanal
        private void InicializarEnvioSMSSemanal()
        {
            var template = ConteudoHTML.RecuperaValorConteudo(BLL.Enumeradores.ConteudoHTML.ConteudoSMSEnvioSemanal);

            var listaSMS = new List<PessoaFisicaEnvioSMSTanque>();
            var idUFPRemetente =
                Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.IdfUfpEnvioSMSTanqueCadastroEmpresa);

            var dt = Curriculo.ListarCurriculosEnvioMensagemSemanal();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    var mensagem = string.Format(template,
                        PessoaFisica.RetornarPrimeiroNome(dr["Nme_Pessoa"].ToString()));

                    var objUsuarioEnvioSMS = new PessoaFisicaEnvioSMSTanque
                    {
                        dddCelular = dr["Num_DDD_Celular"].ToString(),
                        numeroCelular = dr["Num_Celular"].ToString(),
                        nomePessoa = PessoaFisica.RetornarPrimeiroNome(dr["Nme_Pessoa"].ToString()),
                        mensagem = mensagem,
                        idDestinatario = Convert.ToInt32(dr["Idf_Curriculo"])
                    };

                    listaSMS.Add(objUsuarioEnvioSMS);
                }

                Mensagem.EnvioSMSTanque(idUFPRemetente, listaSMS);
            }

            dt.Dispose();
        }
        #endregion

        #region InicializarEnvioSMSSemanalNaoCandidatou
        private void InicializarEnvioSMSSemanalNaoCandidatou()
        {
            var template =
                ConteudoHTML.RecuperaValorConteudo(BLL.Enumeradores.ConteudoHTML.ConteudoSMSEnvioSemanalNaoCandidatou);

            var listaSMS = new List<PessoaFisicaEnvioSMSTanque>();
            var idUFPRemetente =
                Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.IdfUfpEnvioSMSTanqueCadastroEmpresa);

            var dt = Curriculo.ListarCurriculosEnvioMensagemSemanalNaoCandidatou();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    var mensagem = string.Format(template,
                        PessoaFisica.RetornarPrimeiroNome(dr["Nme_Pessoa"].ToString()));

                    var objUsuarioEnvioSMS = new PessoaFisicaEnvioSMSTanque
                    {
                        dddCelular = dr["Num_DDD_Celular"].ToString(),
                        numeroCelular = dr["Num_Celular"].ToString(),
                        nomePessoa = PessoaFisica.RetornarPrimeiroNome(dr["Nme_Pessoa"].ToString()),
                        mensagem = mensagem,
                        idDestinatario = Convert.ToInt32(dr["Idf_Curriculo"])
                    };

                    listaSMS.Add(objUsuarioEnvioSMS);
                }

                Mensagem.EnvioSMSTanque(idUFPRemetente, listaSMS);
            }

            dt.Dispose();
        }
        #endregion

        #endregion
    }
}