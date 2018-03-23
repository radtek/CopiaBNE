using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Reactive.Concurrency;
using System.Threading.Tasks;
using BNE.BLL;
using BNE.BLL.Custom.Email;
using BNE.EL;
using BNE.Services.Base.EventLog;
using BNE.Services.Properties;

namespace BNE.Services
{
    public partial class EnvioSMSEmpresas : BaseService
    {
        #region Métodos

        #region InicializarEnvioSMSEmpresas
        public Task InicializarEnvioSMSEmpresas()
        {
            return Task.Factory.StartNew(() =>
            {
                using (var con = new SqlConnection(DataAccessLayer.CONN_STRING))
                {
                    con.Open();

                    using (var trans = con.BeginTransaction())
                    {
                        var dtFiliaisParaEnviarAvisoSaldoSMS =
                            PlanoAdquirido.RecuperarDadosDasFiliaisParaEnvioAvisoSaldoSMS(trans);
                        if (dtFiliaisParaEnviarAvisoSaldoSMS.Rows.Count > 0)
                        {
                            var IdfUfpAvisoSMS =
                                Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.IdfUfpAvisoSMS, trans);
                            foreach (DataRow filial in dtFiliaisParaEnviarAvisoSaldoSMS.Rows)
                            {
                                //Verifica se algum Usuário ativo da filial tem Plano Selecionadora Ativo, se sim Não enviar SMS para os usuários dessa filial
                                if (
                                    !UsuarioFilialPerfil.PossuiPlanoSelecionadoraAtivo(
                                        Convert.ToInt32(filial["Idf_Filial"]), trans))
                                {
                                    var listaEnvioUsuariosAtivosFilialPerfil =
                                        UsuarioFilialPerfil.CarregarEnvioUsuariosAtivosPorFilialPerfil_SaldoDisponivel(
                                            Convert.ToInt32(filial["Idf_Filial"]), Convert.ToInt32(filial["Qtd_SMS"]),
                                            Convert.ToInt32(filial["Qtd_SMS_Utilizado"]));
                                    if (listaEnvioUsuariosAtivosFilialPerfil != null)
                                        Mensagem.EnvioSMSTanque(IdfUfpAvisoSMS, listaEnvioUsuariosAtivosFilialPerfil,
                                            true);
                                }
                            }
                        }

                        trans.Commit();
                    }
                }
            }
                );
        }
        #endregion

        #endregion

        #region Propriedades
        private static DateTime _dataHoraUltimaExecucao;
        private IDisposable _subscription;
        private EventLogWriter EventLogWriter { get; set; }
        #endregion

        #region Construtores
        public EnvioSMSEmpresas()
        {
            InitializeComponent();
            EventLogWriter = new EventLogWriter(Settings.Default.LogName, GetType().Name);
        }

        public EnvioSMSEmpresas(IContainer container)
            : this()
        {
            container.Add(this);
        }
        #endregion

        #region Eventos

        #region OnStart
        protected override void OnStart(string[] args)
        {
            _subscription = Scheduler.Default.Schedule(GetNextExecution(), next =>
            {
                try
                {
                    InicializarEnvioSMSEmpresas().ContinueWith(t =>
                    {
                        if (t.Status == TaskStatus.RanToCompletion)
                            EventLogWriter.LogEvent(
                                string.Format("Envio AvisoSMSSaldoEmpresas Finalizado: {0}.", DateTime.Now),
                                EventLogEntryType.Information, Event.InicioExecucao);
                        else
                            EventLogWriter.LogEvent(
                                string.Format("Envio AvisoSMSSaldoEmpresas ERRO ao completar ({0}): {1}.", t.Status,
                                    DateTime.Now), EventLogEntryType.Information, Event.InicioExecucao);

                        if (t.Status == TaskStatus.Faulted)
                        {
                            var exp = t.Exception;
                            var details = string.Empty;
                            GerenciadorException.GravarExcecao(exp, details + " (AvisoSMSSaldoEmpresas)");
                            EmailSenderFactory.Create(TipoEnviadorEmail.Fila)
                                .Enviar("appBNE - Erro: Envio AvisoSMSSaldoEmpresas (" + details + ")",
                                    exp.DumpExInternal(), null, "martysroka@bne.com.br", "martysroka@bne.com.br");
                        }
                    });
                }
                finally
                {
                    next(GetNextExecution());
                }
            });
        }

        private DateTimeOffset GetNextExecution()
        {
            var now = DateTime.Now;
            var horaMinuto = Settings.Default.EnvioSMSEmpresasHoraExecucao.Split(':');

            var horaParaExecucao = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                Convert.ToInt32(horaMinuto[0]), Convert.ToInt32(horaMinuto[1]), 0);

            if (horaParaExecucao.Subtract(now).Ticks < 0)
                horaParaExecucao = horaParaExecucao.AddDays(1);

            if (horaParaExecucao.DayOfWeek == DayOfWeek.Saturday)
                horaParaExecucao = horaParaExecucao.AddDays(2);

            if (horaParaExecucao.DayOfWeek == DayOfWeek.Sunday)
                horaParaExecucao = horaParaExecucao.AddDays(1);

            return horaParaExecucao;
        }
        #endregion

        #region OnStop
        protected override void OnStop()
        {
            if (_subscription != null)
                _subscription.Dispose();
        }
        #endregion

        #endregion
    }
}