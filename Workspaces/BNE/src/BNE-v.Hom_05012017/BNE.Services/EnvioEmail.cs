using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Threading;
using BNE.BLL;
using BNE.BLL.Custom.Email;
using BNE.EL;
using BNE.Services.Base.EventLog;
using BNE.Services.Properties;

namespace BNE.Services
{
    internal partial class EnvioEmail : BaseService
    {
        #region Construtores
        public EnvioEmail()
        {
            InitializeComponent();
        }
        #endregion

        #region Propriedades
        private Thread _objThread;
        private static DateTime _dataHoraUltimaExecucao;
        private static readonly int DelayExecucao = Settings.Default.EnvioEmailDelayMinutos;
        #endregion

        #region Eventos

        #region OnStart
        protected override void OnStart(string[] args)
        {
            var objTs = new ThreadStart(IniciarEnvioEmail);
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

        #region IniciarEnvioEmail
        public void IniciarEnvioEmail()
        {
            try
            {
#if (!DEBUG)
                AjustarThread(DateTime.Now);
                #endif

                while (true)
                {
                    _dataHoraUltimaExecucao = DateTime.Now;
                    try
                    {
                        EventLogWriter.LogEvent(string.Format("Iniciou agora {0}.", DateTime.Now),
                            EventLogEntryType.Information, Event.InicioExecucao);

                        var dtMensagens = Mensagem.RecuperarEmailsNaoEnviados();
                        var enviados = new List<int>();
                        var mensagensComErro = new List<int>();

                        try
                        {
                            if (dtMensagens.Rows.Count > 0)
                            {
                                foreach (DataRow row in dtMensagens.Rows)
                                {
                                    try
                                    {
                                        if (row["Arq_Anexo"] != DBNull.Value)
                                        {
                                            if (EmailSenderFactory
                                                .Create(TipoEnviadorEmail.Fila)
                                                .Enviar(Convert.ToString(row["Des_Assunto"]),
                                                    Convert.ToString(row["Des_Mensagem"]),
                                                    null,
                                                    Convert.ToString(row["Des_Email_Remetente"]),
                                                    Convert.ToString(row["Des_Email_Destino"]),
                                                    Convert.ToString(row["Nme_Anexo"]),
                                                    (byte[]) row["Arq_Anexo"]))
                                            {
                                                enviados.Add(Convert.ToInt32(row["Idf_Mensagem_CS"]));
                                                continue;
                                            }
                                        }
                                        else
                                        {
                                            if (EmailSenderFactory
                                                .Create(TipoEnviadorEmail.Fila)
                                                .Enviar(Convert.ToString(row["Des_Assunto"]),
                                                    Convert.ToString(row["Des_Mensagem"]),
                                                    null,
                                                    Convert.ToString(row["Des_Email_Remetente"]),
                                                    Convert.ToString(row["Des_Email_Destino"])))
                                            {
                                                enviados.Add(Convert.ToInt32(row["Idf_Mensagem_CS"]));
                                                continue;
                                            }
                                        }
                                        mensagensComErro.Add(Convert.ToInt32(row["Idf_Mensagem_CS"]));
                                    }
                                    catch (Exception exEnvio)
                                    {
                                        string message;
                                        mensagensComErro.Add(Convert.ToInt32(row["Idf_Mensagem_CS"]));
                                        var id = GerenciadorException.GravarExcecao(exEnvio, out message);
                                        message = string.Format("{0} - {1}", id, message);
                                        EventLogWriter.LogEvent(message, EventLogEntryType.Error, Event.ErroExecucao);
                                    }
                                }

                                if (enviados.Count > 0)
                                {
                                    Mensagem.RemoverMensagens(enviados);
                                }

                                if (mensagensComErro.Count > 0)
                                {
                                    Mensagem.MarcarMensagensComErro(mensagensComErro);
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
                    AjustarThread(DateTime.Now);
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
        private void AjustarThread(DateTime horaAtual)
        {
            var tempoTotalExecucao = horaAtual - _dataHoraUltimaExecucao;
            var delay = new TimeSpan(0, DelayExecucao, 0);
            if ((int) delay.TotalMilliseconds - (int) tempoTotalExecucao.TotalMilliseconds > 0)
            {
                EventLogWriter.LogEvent(
                    string.Format(
                        "Envio Email - Está aguardando {0} para sua próxima execução, pois levou {1} para concluir a excução atual.",
                        delay - tempoTotalExecucao, tempoTotalExecucao), EventLogEntryType.Information,
                    Event.AjusteExecucao);
                Thread.Sleep((int) delay.TotalMilliseconds - (int) tempoTotalExecucao.TotalMilliseconds);
            }
            else
                EventLogWriter.LogEvent(
                    string.Format(
                        "Envio Email - Vai reiniciar agora {0} pois sua última execução foi às {1}, seu tempo de execução foi de {2} e seu delay é de {3}.",
                        DateTime.Now, _dataHoraUltimaExecucao, tempoTotalExecucao, (int) delay.TotalMilliseconds),
                    EventLogEntryType.Warning, Event.WarningAjusteExecucao);
        }
        #endregion

        #endregion
    }
}