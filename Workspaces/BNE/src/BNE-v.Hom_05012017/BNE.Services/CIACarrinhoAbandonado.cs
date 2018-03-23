using BNE.BLL;
using BNE.BLL.Custom;
using BNE.EL;
using BNE.Services.Base.EventLog;
using BNE.Services.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;

namespace BNE.Services
{
    internal partial class CIACarrinhoAbandonado : BaseService
    {
        #region [ Construtores ]
        public CIACarrinhoAbandonado()
        {
            InitializeComponent();
        }
        #endregion

        #region Propriedades
        private Thread _objThread;
        private static DateTime _dataHoraUltimaExecucao;
        private static readonly string HoraExecucao = Settings.Default.CIACarrinhoAbandonadoHoraExecucao;
        private static readonly int DelayExecucao = Settings.Default.CIACarrinhoAbandonadoDelayMinutos;
        #endregion

        protected override void OnStart(string[] args)
        {
            var objTs = new ThreadStart(IniciarEnvioEmailAlertaCarrinhoAbandonado);
            _objThread = new Thread(objTs);
            _objThread.Start();
        }

        protected override void OnStop()
        {
            if (_objThread != null)
                _objThread.Abort();
        }

        #region Metodos

        #region IniciarEnvioEmailAlertaCarrinhoAbandonado
        public void IniciarEnvioEmailAlertaCarrinhoAbandonado()
        {
            try
            {
                AjustarThread(DateTime.Now, true);

                while (true)
                {
                    _dataHoraUltimaExecucao = DateTime.Now;
                    try
                    {
                        EventLogWriter.LogEvent(string.Format("Iniciou agora {0}.", DateTime.Now),
                            EventLogEntryType.Information, Event.InicioExecucao);
                        try
                        {
                            //buscar empresas que abandonaram a compra
                            var dtCompra = Filial.FilialCarrinhaAbandonado();

                            foreach (DataRow compra in dtCompra.Rows)
                            {
                                var emailVendedor = compra["Eml_Vendedor"].ToString();
                                var objcarta = CartaEmail.LoadObject(Convert.ToInt32(BLL.Enumeradores.CartaEmail.CIACarrinhoAbandonado));
                                var cartaEmail = CartaEmail.RecuperarConteudo(BLL.Enumeradores.CartaEmail.CIACarrinhoAbandonado);

                                cartaEmail = cartaEmail.Replace("{nome}", compra["Nme_Pessoa"].ToString());
                                cartaEmail = cartaEmail.Replace("{vendedor}", compra["Nme_Vendedor"].ToString());

                                var objUsuarioFilialPerfil = UsuarioFilialPerfil.LoadObject(Convert.ToInt32(compra["Idf_Usuario_Filial_Perfil"]));

                                var objMensagem = new MensagemSistema();
                                objMensagem.DescricaoMensagemSistema = cartaEmail;

                                if (Validacao.ValidarEmail(compra["Eml_Comercial"].ToString()))
                                {
                                    //Enviar E-mail para a empresa
                                    MensagemCS.SalvarEmail(null, null, objUsuarioFilialPerfil, null,
                                        objcarta.DescricaoAssunto, objMensagem.DescricaoMensagemSistema, BLL.Enumeradores.CartaEmail.CIACarrinhoAbandonado, emailVendedor,
                                        compra["Eml_Comercial"].ToString(), null, null, null);
                                }
                            }
                        }
                        catch (Exception exEnvio)
                        {
                            string message;
                            var id = GerenciadorException.GravarExcecao(exEnvio, out message);
                            message = string.Format("{0} - {1}", id, message);
                            EventLogWriter.LogEvent(message, EventLogEntryType.Error, Event.ErroExecucao);
                        }
                    }
                    catch (Exception ex)
                    {
                        string message;
                        var id = GerenciadorException.GravarExcecao(ex, out message);
                        message = string.Format("{0} - {1}", id, message);

                        EventLogWriter.LogEvent(message, EventLogEntryType.Error, Event.ErroExecucao);
                    }
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
                Thread.Sleep((int)tempoParaExecutar.TotalMilliseconds);
            }
            else
            {
                var tempoTotalExecucao = horaAtual - _dataHoraUltimaExecucao;
                var delay = new TimeSpan(0, DelayExecucao, 0);
                if ((int)delay.TotalMilliseconds - (int)tempoTotalExecucao.TotalMilliseconds > 0)
                {
                    EventLogWriter.LogEvent(
                        string.Format(
                            "Está aguardando {0} para sua próxima execução, pois levou {1} para concluir a excução atual.",
                            delay - tempoTotalExecucao, tempoTotalExecucao), EventLogEntryType.Information,
                        Event.AjusteExecucao);
                    Thread.Sleep((int)delay.TotalMilliseconds - (int)tempoTotalExecucao.TotalMilliseconds);
                }
                else
                    EventLogWriter.LogEvent(
                        string.Format(
                            "Vai reiniciar agora {0} pois sua última execução foi às {1}, seu tempo de execução foi de {2} e seu delay é de {3}.",
                            DateTime.Now, _dataHoraUltimaExecucao, tempoTotalExecucao, (int)delay.TotalMilliseconds),
                        EventLogEntryType.Warning, Event.WarningAjusteExecucao);
            }
        }
        #endregion

        #endregion

    }
}
