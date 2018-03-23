using BNE.BLL;
using System;
using System.Data;
using System.ServiceProcess;
using System.Threading;
using BNE.Services.Properties;
using BNE.EL;
using BNE.Services.Code;
using System.Diagnostics;
using System.Collections.Generic;

namespace BNE.Services
{
    partial class EnvioSMSSemanal : ServiceBase
    {

        #region Propriedades
        private Thread _objThread;
        private static readonly string HoraExecucao = Settings.Default.EnvioSMSSemanalHoraExecucao;
        private static readonly int DelayExecucao = Settings.Default.EnvioSMSSemanalDelayMinutos;
        private const string EventSourceName = "EnvioSMSSemanal";
        private static DateTime _dataHoraUltimaExecucao;
        #endregion

        #region Construtores
        public EnvioSMSSemanal()
        {
            InitializeComponent();
        }
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

                        EventLogWriter.LogEvent(EventSourceName, String.Format("Iniciou agora {0}.", DateTime.Now), EventLogEntryType.Information, (int)EventID.InicioExecucao);

                        InicializarEnvioSMSSemanal();
                        InicializarEnvioSMSSemanalNaoCandidatou();

                        EventLogWriter.LogEvent(EventSourceName, String.Format("Terminou agora {0}.", DateTime.Now), EventLogEntryType.Information, (int)EventID.FimExecucao);

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
                EventLogWriter.LogEvent(EventSourceName, message, EventLogEntryType.Error, (int)EventID.ErroExecucao);
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
                EventLogWriter.LogEvent(EventSourceName, String.Format("Envio SMS Semanal - Está aguardando {0} para sua iniciar sua execução.", tempoParaExecutar.ToString()), EventLogEntryType.Information, (int)EventID.AjusteExecucao);
                Thread.Sleep((int)tempoParaExecutar.TotalMilliseconds);
            }
            else
            {
                TimeSpan tempoTotalExecucao = horaAtual - _dataHoraUltimaExecucao;
                var delay = new TimeSpan(0, DelayExecucao, 0);
                if ((int)delay.TotalMilliseconds - (int)tempoTotalExecucao.TotalMilliseconds > 0)
                {
                    EventLogWriter.LogEvent(EventSourceName, String.Format("Envio SMS Semanal - Está aguardando {0} para sua próxima execução, pois levou {1} para concluir a excução atual.", delay - tempoTotalExecucao, tempoTotalExecucao), EventLogEntryType.Information, (int)EventID.AjusteExecucao);
                    Thread.Sleep((int)delay.TotalMilliseconds - (int)tempoTotalExecucao.TotalMilliseconds);
                }
                else
                    EventLogWriter.LogEvent(EventSourceName, String.Format("Envio SMS Semanal - Vai reiniciar agora {0} pois sua última execução foi às {1}, seu tempo de execução foi de {2} e seu delay é de {3}.", DateTime.Now, _dataHoraUltimaExecucao, tempoTotalExecucao, (int)delay.TotalMilliseconds), EventLogEntryType.Warning, (int)EventID.WarningAjusteExecucao);
            }
        }
        #endregion

        #region InicializarEnvioSMSSemanal
        private void InicializarEnvioSMSSemanal()
        {
            string template = ConteudoHTML.RecuperaValorConteudo(BLL.Enumeradores.ConteudoHTML.ConteudoSMSEnvioSemanal);

            List<BNE.BLL.DTO.PessoaFisicaEnvioSMSTanque> listaSMS = new List<BLL.DTO.PessoaFisicaEnvioSMSTanque>();
            string idUFPRemetente = Parametro.RecuperaValorParametro(BNE.BLL.Enumeradores.Parametro.IdfUfpEnvioSMSTanqueCadastroEmpresa);

            DataTable dt = Curriculo.ListarCurriculosEnvioMensagemSemanal();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    string mensagem = string.Format(template, PessoaFisica.RetornarPrimeiroNome(dr["Nme_Pessoa"].ToString()));

                    var objUsuarioEnvioSMS = new BNE.BLL.DTO.PessoaFisicaEnvioSMSTanque {
                        dddCelular = dr["Num_DDD_Celular"].ToString(),
                        numeroCelular = dr["Num_Celular"].ToString(),
                        nomePessoa = PessoaFisica.RetornarPrimeiroNome(dr["Nme_Pessoa"].ToString()),
                        mensagem = mensagem,
                        idDestinatario = Convert.ToInt32(dr["Idf_Curriculo"])
                    };

                    listaSMS.Add(objUsuarioEnvioSMS);
                }

                BNE.BLL.Mensagem.EnvioSMSTanque(idUFPRemetente, listaSMS);
            }

            dt.Dispose();
        }
        #endregion

        #region InicializarEnvioSMSSemanalNaoCandidatou
        private void InicializarEnvioSMSSemanalNaoCandidatou()
        {
            string template = ConteudoHTML.RecuperaValorConteudo(BLL.Enumeradores.ConteudoHTML.ConteudoSMSEnvioSemanalNaoCandidatou);

            List<BNE.BLL.DTO.PessoaFisicaEnvioSMSTanque> listaSMS = new List<BLL.DTO.PessoaFisicaEnvioSMSTanque>();
            string idUFPRemetente = Parametro.RecuperaValorParametro(BNE.BLL.Enumeradores.Parametro.IdfUfpEnvioSMSTanqueCadastroEmpresa);

            DataTable dt = Curriculo.ListarCurriculosEnvioMensagemSemanalNaoCandidatou();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    string mensagem = string.Format(template, PessoaFisica.RetornarPrimeiroNome(dr["Nme_Pessoa"].ToString()));

                    var objUsuarioEnvioSMS = new BNE.BLL.DTO.PessoaFisicaEnvioSMSTanque
                    {
                        dddCelular = dr["Num_DDD_Celular"].ToString(),
                        numeroCelular = dr["Num_Celular"].ToString(),
                        nomePessoa = PessoaFisica.RetornarPrimeiroNome(dr["Nme_Pessoa"].ToString()),
                        mensagem = mensagem,
                        idDestinatario = Convert.ToInt32(dr["Idf_Curriculo"])
                    };

                    listaSMS.Add(objUsuarioEnvioSMS);
                }

                BNE.BLL.Mensagem.EnvioSMSTanque(idUFPRemetente, listaSMS);
            }

            dt.Dispose();
        }
        #endregion

        #endregion

    }
}
