using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using BNE.BLL;
using BNE.BLL.BNETanqueService;
using BNE.BLL.Common;
using BNE.BLL.Custom;
using BNE.BLL.Integracoes.Google;
using BNE.EL;
using BNE.Services.Base.EventLog;
using BNE.Services.Properties;
using Mensagem = BNE.BLL.BNETanqueService.Mensagem;

namespace BNE.Services
{
    public partial class EnvioSMSParceirosLAN : BaseService
    {
        public EnvioSMSParceirosLAN()
        {
            InitializeComponent();
        }

        #region Propriedades
        private const string DIRECT_URL = @"http://www.bne.com.br/vaga-de-emprego-na-area-a-em-a-aa/a/{0}";
        private Thread _objThread;
        private static DateTime _dataHoraUltimaExecucao;
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

        #region Métodos

        #region Iniciar
        public void Iniciar()
        {
            try
            {
                Settings.Default.Reload();
                AjustarThread();
                while (true)
                {
                    try
                    {
                        EventLogWriter.LogEvent(string.Format("EnvioSMSParceirosLAN - Iniciada: {0}.", DateTime.Now),
                            EventLogEntryType.Information, Event.InicioExecucao);
                        _dataHoraUltimaExecucao = DateTime.Now;
                        foreach (var usuario in UsuarioParceiro.RecuperarPagoComEnvio())
                        {
                            try
                            {
                                var celular = string.Format("{0}{1}", usuario.NumDDDCelular.Trim(),
                                    usuario.NumCelular.Trim());
                                var f = usuario.RecuperarUltimaFuncaoId();
                                var surl = "";
                                if (f > 0)
                                {
                                    surl = EncurtadorDeURL.Encurtar(string.Format(DIRECT_URL, f));
                                    var msgSMS =
                                        string.Format(
                                            "{0}, confira a nova vaga que encontramos para você, acesse {1} e candidate-se. Sucesso!",
                                            usuario.NmeUsuario.Split(' ')[0].Trim().RemoveDiacritics(), surl);
                                    EnviarSMS(msgSMS, usuario.NmeUsuario, celular);
                                    usuario.UpdateEnvioSMS();
                                }
                            }
                            catch (Exception ex)
                            {
                                GravaLogErro(ex);
                            }
                        }
                        EventLogWriter.LogEvent(
                            string.Format("EnvioSMSParceirosLAN Terminou agora {0}.", DateTime.Now),
                            EventLogEntryType.Information, Event.FimExecucao);
                    }
                    catch (Exception ex)
                    {
                        GravaLogErro(ex);
                    }

                    AjustarThread();
                }
            }
            catch (Exception ex)
            {
                GravaLogErro(ex);
            }
        }
        #endregion

        #region EnviarSMS
        private void EnviarSMS(string msg, string nomePessoa, string celular)
        {
            using (var objWsTanque = new AppClient())
            {
                objWsTanque.Open();
                var listaSMS = new List<Mensagem>();

                var mensagem = new Mensagem();
                mensagem.ci = celular.Trim();
                mensagem.np = nomePessoa.Trim();
                mensagem.nc = Convert.ToDecimal(celular.Trim());
                mensagem.dm = msg;

                listaSMS.Add(mensagem);
                var receberMensagem = new InReceberMensagem {l = listaSMS.ToArray(), cu = "EnvioSMSParceirosLAN"};
                try
                {
                    var retorno = objWsTanque.ReceberMensagem(receberMensagem);
                    if (retorno.l == null)
                        GravaLogErro(new Exception("Problema ao enviar SMS em EnvioSMSParceirosLAN"));
                    else if (retorno.l.Count() == 0)
                        GravaLogErro(new Exception("Problema ao enviar SMS em EnvioSMSParceirosLAN"));
                }
                catch (Exception ex)
                {
                    GravaLogErro(ex);
                    throw ex;
                }
            }
        }
        #endregion

        #region GravaLogErro
        protected void GravaLogErro(Exception ex)
        {
            try
            {
                var message = string.Format("appBNE - Hora: {0}. Erro: {1}", DateTime.Now.ToLongTimeString(), ex);
                EventLogWriter.LogEvent(message, EventLogEntryType.Error, Event.ErroExecucao);
                GerenciadorException.GravarExcecao(ex);
            }
            catch (Exception)
            {
            }
        }
        #endregion

        #region AjustarThread
        private void AjustarThread()
        {
            var horaAtual = DateTime.Now;
            Settings.Default.Reload();
            var HoraExecucao = Settings.Default.EnvioSMSParceirosInicioExecucao;

            var horaMinuto = HoraExecucao.Split(':');
            var horaParaExecucao = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                Convert.ToInt32(horaMinuto[0]), Convert.ToInt32(horaMinuto[1]), 0);

            if (horaParaExecucao.Subtract(horaAtual).TotalMilliseconds < 0)
                horaParaExecucao = horaParaExecucao.AddDays(1);

            var diasPermitidos = new List<DayOfWeek> {DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Friday};

            while (!diasPermitidos.Contains(horaParaExecucao.DayOfWeek))
                horaParaExecucao = horaParaExecucao.AddDays(1);

            var tempoParaExecutar = horaParaExecucao - horaAtual;

            EventLogWriter.LogEvent(
                string.Format("O Serviço está aguardando {0} para sua iniciar sua execução.", tempoParaExecutar),
                EventLogEntryType.Information, Event.AjusteExecucao);
            Thread.Sleep((int) tempoParaExecutar.TotalMilliseconds);
        }
        #endregion

        #endregion
    }
}