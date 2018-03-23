using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using BNE.BLL;
using BNE.BLL.Custom.Vaga;
using BNE.EL;
using BNE.Services.Base.EventLog;
using BNE.Services.Code.Integrador;
using BNE.Services.Properties;
using Parametro = BNE.BLL.Enumeradores.Parametro;

namespace BNE.Services
{
    internal partial class IntegrarVagas : BaseService
    {
        #region Construtores
        public IntegrarVagas()
        {
            InitializeComponent();
        }
        #endregion Construtores

        #region Help
        /*
         *  Classe responsavel por:
         *  1 - Buscar as vagas dos XMLs (RIPPER e Feeds Online);
         *  2 - Enfileirar via Queue todas as vagas buscadas, para enviar ao banco de dados do BNE;
         *  
         */
        #endregion Help

        #region Propriedades
        private Thread _objThread;
        private static DateTime _dataHoraUltimaExecucao;
        private Stopwatch watch;
        #endregion Propriedades

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

        #region Iniciar
        public void Iniciar()
        {
            try
            {
                Settings.Default.Reload();
                AjustarThread(DateTime.Now, true);

                while (true)
                {
                    try
                    {
                        watch = new Stopwatch();
                        watch.Start();

                        EnviaLogEmail("Integração de Vagas Assincrona - Iniciando",
                            "Todas as vagas de todas as origens - Iniciando", "Iniciar()");
                        EventLogWriter.LogEvent(
                            string.Format("Integração de Vagas Assincrona - Iniciada: {0}.", DateTime.Now),
                            EventLogEntryType.Information, Event.InicioExecucao);
                        _dataHoraUltimaExecucao = DateTime.Now;

                        foreach (var objOrigemImportacao in Integrador.RecuperarIntegradoresAtivos())
                        {
                            try
                            {
                                RealizarImportacao(objOrigemImportacao);
                            }
                            catch (Exception ex)
                            {
                                GravaLogErro(ex, "Iniciar()");
                            }
                        }

                        //Grava os Logs de sucesso
                        EventLogWriter.LogEvent(string.Format("Terminou agora {0}.", DateTime.Now),
                            EventLogEntryType.Information, Event.FimExecucao);
                        watch.Stop();
                        EnviaLogEmail("Integração de Vagas Assincrona - Finalizado",
                            "Todas as vagas de todas as origens", "Iniciar()", watch.Elapsed.Minutes.ToString());
                    }
                    catch (Exception ex)
                    {
                        GravaLogErro(ex, "Iniciar()");
                    }

                    AjustarThread(DateTime.Now, false);
                }
            }
            catch (Exception ex)
            {
                EnviaLogEmail("!!!!!!!Fim da execução do Robo de Vagas!!!!!!!", ex.ToString(), "Iniciar()");
            }
        }
        #endregion

        #region RealizarImportacao
        private void RealizarImportacao(Integrador objIntegrador)
        {
            try
            {

                var origem = DefineOrigem(objIntegrador);

                var vagas = Service.EfetuarRequisicaoAssincrona(objIntegrador).ToList();

                foreach (var objVagaIntegracaoImportada in vagas)
                {
                    try
                    {
                        //Enfileira a vagaImportacao para que seja processada.
                        EnfileiraVaga.EnfileiraByVagaIntegracao(objVagaIntegracaoImportada, origem, objIntegrador);
                    }
                    catch (Exception ex)
                    {
                        GravaLogErro(ex, "RealizarImportacao()");
                    }
                }
            }
            catch (Exception ex)
            {
                GravaLogErro(ex, "RealizarImportacao()");
            }
            finally
            {
            }
        }
        #endregion

        #region DefineOrigem
        protected Origem DefineOrigem(Integrador objIntegrador)
        {
            //Recuperando parametrização.
            var sIdOrigem = objIntegrador.GetValorParametro(Parametro.Integracao_Idf_Origem_Vaga);
            int origem;
            Origem objOrigem;

            //Verifica parametrização do integrador
            if (!string.IsNullOrEmpty(sIdOrigem) && int.TryParse(sIdOrigem, out origem))
                objOrigem = new Origem(origem);
            else
            {
                //Recupera a origem da filial do Integrador para definir corretamente a Origem
                var idOrigem = OrigemFilial.RecuperarIdOrigemPorFilial(objIntegrador.Filial);
                objOrigem = idOrigem.HasValue ? new Origem(idOrigem.Value) : new Origem(1);
                    //Se não foi encontrada origem para a filial, a origem será o BNE
            }
            return objOrigem;
        }
        #endregion DefineOrigem

        #region AjustarThread
        private void AjustarThread(DateTime horaAtual, bool primeiraExecucao)
        {
            Settings.Default.Reload();
            var DelayExecucao = Settings.Default.IntegrarVagasDelayMinutos;
            var HoraExecucao = Settings.Default.IntegrarVagasInicioJanelaExecucao;

            if (primeiraExecucao)
            {
                var horaMinuto = HoraExecucao.Split(':');

                var horaParaExecucao = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                    Convert.ToInt32(horaMinuto[0]), Convert.ToInt32(horaMinuto[1]), 0);

                if (horaParaExecucao.Subtract(horaAtual).TotalMilliseconds < 0)
                    horaParaExecucao = horaParaExecucao.AddDays(1);

                var tempoParaExecutar = horaParaExecucao - horaAtual;

                EventLogWriter.LogEvent(
                    string.Format("O Serviço está aguardando {0} para sua iniciar sua execução.", tempoParaExecutar),
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
                            "O Serviço está aguardando {0} para sua próxima execução, pois levou {1} para concluir a excução atual.",
                            delay - tempoTotalExecucao, tempoTotalExecucao), EventLogEntryType.Information,
                        Event.AjusteExecucao);
                    Thread.Sleep((int) delay.TotalMilliseconds - (int) tempoTotalExecucao.TotalMilliseconds);
                }
                else
                    EventLogWriter.LogEvent(
                        string.Format(
                            "O Serviço vai reiniciar agora {0} pois sua última execução foi às {1}, seu tempo de execução foi de {2} e seu delay é de {3}.",
                            DateTime.Now, _dataHoraUltimaExecucao, tempoTotalExecucao, (int) delay.TotalMilliseconds),
                        EventLogEntryType.Warning, Event.WarningAjusteExecucao);
            }
        }
        #endregion

        #region Logs
        protected static void EnviaLogEmail(string assunto, string mensagem, string metodo,
            string tempoExec = "Não Especificado")
        {
            try
            {
                assunto = "appBNE - Robo de Vagas: " + assunto;
                mensagem = "appBNE - Robo de Vagas: " + mensagem;

                var sMensagem = new StringBuilder();
                sMensagem.Append(" <br> Método: " + metodo);

                if (!string.IsNullOrEmpty(tempoExec))
                    sMensagem.Append(" <br> Tempo Execução: " + tempoExec + " Minutos");

                sMensagem.Append(" <br> Mensagem: " + mensagem);

                MensagemCS.SalvarEmail(null, null, null, null, assunto, sMensagem.ToString(),null, "martysroka@bne.com.br",
                    "martysroka@bne.com.br", null, null, null);
            }
            catch (Exception)
            {
            }
        }

        protected void GravaLogErro(Exception ex, string metodo)
        {
            try
            {
                var message = string.Format("appBNE - Erro no Robo de Importacao de Vagas. Hora: {0}. Erro: {1}",
                    DateTime.Now.ToLongTimeString(), ex);
                EventLogWriter.LogEvent(message, EventLogEntryType.Error, Event.ErroExecucao);

                var sCustomMessage = "appBNE - Integracao Vagas. Erro no Robo de Importacao de Vagas. Método: " + metodo;
                GerenciadorException.GravarExcecao(ex, sCustomMessage);
            }
            catch (Exception)
            {
            }
        }
        #endregion TempMonitoramento

        #endregion
    }
}