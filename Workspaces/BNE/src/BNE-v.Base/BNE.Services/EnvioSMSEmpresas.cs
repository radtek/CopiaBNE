using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

using BNE.BLL;
using System.Data;
using System.ServiceProcess;
using System.Threading;
using BNE.Services.Properties;
using BNE.EL;
using BNE.Services.Code;
using System.Reactive.Concurrency;
using System.Threading.Tasks;
using BNE.BLL.Custom.Email;

namespace BNE.Services
{
    public partial class EnvioSMSEmpresas : ServiceBase
    {
        public EnvioSMSEmpresas(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        #region Propriedades
        private static readonly string HoraExecucao = Settings.Default.EnvioSMSEmpresasHoraExecucao;        
        private const string EventSourceName = "EnvioSMSEmpresas";
        private static DateTime _dataHoraUltimaExecucao;
        private IDisposable _subscription;
        #endregion

        #region Construtores
        public EnvioSMSEmpresas()
        {
            InitializeComponent();
        }
        #endregion

        #region Eventos

        #region OnStart
        protected override void OnStart(string[] args)
        {
            _subscription = Scheduler.Default.Schedule(GetNextExecution(), (next) =>
            {
                try
                {
                    
                    InicializarEnvioSMSEmpresas().ContinueWith(t =>
                    {
                        if (t.Status == TaskStatus.RanToCompletion)
                            EventLogWriter.LogEvent(EventSourceName, String.Format("Envio AvisoSMSSaldoEmpresas Finalizado: {0}.", DateTime.Now), EventLogEntryType.Information, (int)EventID.InicioExecucao);
                        else
                            EventLogWriter.LogEvent(EventSourceName, String.Format("Envio AvisoSMSSaldoEmpresas ERRO ao completar ({0}): {1}.", t.Status, DateTime.Now), EventLogEntryType.Information, (int)EventID.InicioExecucao);

                        if (t.Status == TaskStatus.Faulted)
                        {
                            var exp = t.Exception;
                            var details = string.Empty;
                            EL.GerenciadorException.GravarExcecao(exp, details + " (AvisoSMSSaldoEmpresas)");
                            EmailSenderFactory.Create(TipoEnviadorEmail.Fila).Enviar("appBNE - Erro: Envio AvisoSMSSaldoEmpresas (" + details + ")", exp.DumpExInternal(), "valerianeves@bne.com.br", "valerianeves@bne.com.br");
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
            string[] horaMinuto = Settings.Default.EnvioSMSEmpresasHoraExecucao.Split(':');

            DateTime horaParaExecucao = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Convert.ToInt32(horaMinuto[0]), Convert.ToInt32(horaMinuto[1]), 0);

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

        #region Métodos

        #region InicializarEnvioSMSEmpresas
        public Task InicializarEnvioSMSEmpresas()
        {
            return Task.Factory.StartNew(() => {
                    using (var con = new System.Data.SqlClient.SqlConnection(DataAccessLayer.CONN_STRING))
                    {
                        con.Open();
                        
                        using (var trans = con.BeginTransaction())
                        {
                            var dtFiliaisParaEnviarAvisoSaldoSMS = PlanoAdquirido.RecuperarDadosDasFiliaisParaEnvioAvisoSaldoSMS(trans);
                            if (dtFiliaisParaEnviarAvisoSaldoSMS.Rows.Count > 0)
                            {
                                var IdfUfpAvisoSMS = Parametro.RecuperaValorParametro(BNE.BLL.Enumeradores.Parametro.IdfUfpAvisoSMS, trans);
                                foreach (DataRow filial in dtFiliaisParaEnviarAvisoSaldoSMS.Rows)
                                {
                                    //Verifica se algum Usuário ativo da filial tem Plano Selecionadora Ativo, se sim Não enviar SMS para os usuários dessa filial
                                    if(!UsuarioFilialPerfil.PossuiPlanoSelecionadoraAtivo(Convert.ToInt32(filial["Idf_Filial"]), trans))
                                    {
                                        List<BNE.BLL.DTO.PessoaFisicaEnvioSMSTanque> listaEnvioUsuariosAtivosFilialPerfil = UsuarioFilialPerfil.CarregarEnvioUsuariosAtivosPorFilialPerfil_SaldoDisponivel(Convert.ToInt32(filial["Idf_Filial"]), Convert.ToInt32(filial["Qtd_SMS"]), Convert.ToInt32(filial["Qtd_SMS_Utilizado"]));
                                        if (listaEnvioUsuariosAtivosFilialPerfil != null)
                                            Mensagem.EnvioSMSTanque(IdfUfpAvisoSMS, listaEnvioUsuariosAtivosFilialPerfil, true);
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
    }
}
