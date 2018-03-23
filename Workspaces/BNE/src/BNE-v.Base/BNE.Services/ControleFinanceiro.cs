using BNE.BLL;
using BNE.BLL.Custom;
using BNE.BLL.Integracoes.WFat;
using BNE.Services.Code;
using BNE.Services.Properties;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.ServiceProcess;
using System.Text;
using System.Threading;

namespace BNE.Services
{
    /// <summary>
    /// 
    /// </summary>
    partial class ControleFinanceiro : ServiceBase
    {

        #region Propriedades
        private Thread _objThread;
        private static DateTime _dataHoraUltimaExecucao;
        private static int DelayExecucao = Settings.Default.ControleFinanceiroDelayMinutos;
        private const string EventSourceName = "ControleFinanceiro";
        #endregion

        #region Construtor
        public ControleFinanceiro()
        {
            InitializeComponent();
        }
        #endregion

        #region Eventos
        #region OnStart
        protected override void OnStart(string[] args)
        {
            (_objThread = new Thread(new ThreadStart(Iniciar))).Start();
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
            Settings.Default.Reload();
            DelayExecucao = Settings.Default.ControleFinanceiroDelayMinutos;

            while (true)
            {
                _dataHoraUltimaExecucao = DateTime.Now;
#if !DEBUG
                EventLogWriter.LogEvent(EventSourceName, String.Format("Iniciou agora {0}.", DateTime.Now), EventLogEntryType.Information, (int)EventID.InicioExecucao);
                EventLogWriter.LogEvent(EventSourceName, String.Format("Capturando trasações não capturadas: {0}.", DateTime.Now), EventLogEntryType.Information, (int)EventID.InicioExecucao);
#endif
                Settings.Default.Reload();

                try
                {
                    CapturarTransacoesNaoCapturadas();
                }
                catch (Exception ex)
                {
#if !DEBUG
                    EventLogWriter.LogEvent(EventSourceName, ex.Message, EventLogEntryType.Error, (int)EventID.FimExecucao);
#endif
                    EL.GerenciadorException.GravarExcecao(ex);
                }
#if !DEBUG
                EventLogWriter.LogEvent(EventSourceName, String.Format("Obtendo notas: {0}.", DateTime.Now), EventLogEntryType.Information, (int)EventID.InicioExecucao);
#endif
                try
                {
                    ObterNotas();
                }
                catch (Exception ex)
                {
                    EventLogWriter.LogEvent(EventSourceName, ex.Message, EventLogEntryType.Error, (int)EventID.FimExecucao);
                    EL.GerenciadorException.GravarExcecao(ex);
                }
#if !DEBUG
                EventLogWriter.LogEvent(EventSourceName, String.Format("Gerando notas para os pagamentos: {0}.", DateTime.Now), EventLogEntryType.Information, (int)EventID.InicioExecucao);
#endif
                try
                {
                    GerarNotas();
                }
                catch (Exception ex)
                {
                    EventLogWriter.LogEvent(EventSourceName, ex.Message, EventLogEntryType.Error, (int)EventID.FimExecucao);
                    EL.GerenciadorException.GravarExcecao(ex);
                }
#if !DEBUG
                EventLogWriter.LogEvent(EventSourceName, String.Format("Terminou agora {0}.", DateTime.Now), EventLogEntryType.Information, (int)EventID.FimExecucao);    
                AjustarThread(DateTime.Now, false);
#endif
            }
        }
        #endregion

        #region CapturarTransacoesNaoCapturadas
        /// <summary>
        /// Realiza a liberação e a captura para cartões aprovados mas não liberados
        /// </summary>
        private void CapturarTransacoesNaoCapturadas()
        {
            try
            {
                List<Transacao> lst = Transacao.CarregarTransacoesCreditoNaoCapturadas();

                foreach (Transacao objTransacao in lst)
                {
                    try
                    {
                        //Criando a transação 
                        var objPagamento = Pagamento.LoadObject(objTransacao.Pagamento.IdPagamento);
                        //Se foi aprovado liberar o pagamento
                        if (objPagamento.Liberar(DateTime.Now))
                        {
                            objTransacao.CapturarTransacaoCartao();
                        }
                    }
                    catch (Exception ex)
                    {
                        EL.GerenciadorException.GravarExcecao(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
            }
            return;
        }

        #endregion

        #region GerarNotas
        /// <summary>
        /// Gera as notas pendentes
        /// </summary>
        private void GerarNotas()
        {
            try
            {
                List<Pagamento> lst = Pagamento.CarregarPagosSemNota();

                #if !DEBUG
                EventLogWriter.LogEvent(EventSourceName, String.Format("{0} notas para serem geradas.", lst.Count), EventLogEntryType.Information, (int)EventID.FimExecucao);
                #endif

                int cont = 0;
                foreach (Pagamento objPagamento in lst)
                {
                    try
                    {
                        #if !DEBUG
                        if (cont % 10 == 0)
                            EventLogWriter.LogEvent(EventSourceName, String.Format("{0}/{1} notas geradas.", cont, lst.Count), EventLogEntryType.Information, (int)EventID.FimExecucao);
                        #endif
                        // @TODO Armazenar no pagamento o usuario filial perfil que liberou o plano
                        PlanoParcela.EmitirNF(objPagamento, 3424172, null);

                        cont++;
                    }
                    catch (Exception ex)
                    {
                        EventLogWriter.LogEvent(EventSourceName, ex.Message, EventLogEntryType.Error, (int)EventID.ErroExecucao);
                        EL.GerenciadorException.GravarExcecao(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                EventLogWriter.LogEvent(EventSourceName, ex.Message, EventLogEntryType.Error, (int)EventID.FimExecucao);
                EL.GerenciadorException.GravarExcecao(ex);
            }
            return;
        }
        #endregion GerarNotas

        #region ObterNotas
        /// <summary>
        /// Busca as notas ainda não importadas do plataforma para o BNE
        /// </summary>
        private void ObterNotas()
        {
            List<NotaFiscal> lstNota = NotaFiscal.ObterNotasNaoImportadas();

          foreach (NotaFiscal nota in lstNota)
            {
                Pagamento objPagamento;
                if (Pagamento.CarregarPagamentoPorNossoNumeroBoleto(nota.Transacao, out objPagamento))
                {
                    objPagamento.SalvarNotaFiscal(nota.NumeroNotaFiscal.ToString(), nota.Link);
                }
            }
        }
        #endregion ObterNotas

        #region AjustarThread
        private void AjustarThread(DateTime horaAtual, bool primeiraExecucao)
        {
            if (!primeiraExecucao)
            {
                var tempoTotalExecucao = horaAtual - _dataHoraUltimaExecucao;
                var delay = new TimeSpan(0, DelayExecucao, 0);
                if ((int)delay.TotalMilliseconds - (int)tempoTotalExecucao.TotalMilliseconds > 0)
                {
                    EventLogWriter.LogEvent(EventSourceName, String.Format("Está aguardando {0} para sua próxima execução, pois levou {1} para concluir a excução atual.", delay - tempoTotalExecucao, tempoTotalExecucao), EventLogEntryType.Information, (int)EventID.AjusteExecucao);
                    Thread.Sleep((int)delay.TotalMilliseconds - (int)tempoTotalExecucao.TotalMilliseconds);
                }
                else
                    EventLogWriter.LogEvent(EventSourceName, String.Format("Vai reiniciar agora {0} pois sua última execução foi às {1}, seu tempo de execução foi de {2} e seu delay é de {3}.", DateTime.Now, _dataHoraUltimaExecucao, tempoTotalExecucao, (int)delay.TotalMilliseconds), EventLogEntryType.Warning, (int)EventID.WarningAjusteExecucao);
            }
        }
        #endregion

        #endregion
    }
}
