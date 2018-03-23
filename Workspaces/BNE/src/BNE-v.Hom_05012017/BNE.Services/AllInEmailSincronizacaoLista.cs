using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.FtpClient;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AllInMail.Core;
using AllInMail.Core.Model;
using BNE.BLL;
using BNE.BLL.Custom.Email;
using BNE.EL;
using BNE.Services.Base.EventLog;
using BNE.Services.Code;
using BNE.Services.Properties;

namespace BNE.Services
{
    internal partial class AllInEmailSincronizacaoLista : BaseService
    {
        #region [ Attributes / Fields ]
        private IDisposable _subscription;
        #endregion

        private DateTimeOffset GetNextExecution()
        {
            var now = DateTime.Now;
            var horaMinuto = Settings.Default.AllInAtualizarListaEmails.Split(':');

            var horaParaExecucao = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                Convert.ToInt32(horaMinuto[0]), Convert.ToInt32(horaMinuto[1]), 0);

            if (horaParaExecucao.Subtract(now).Ticks < 0)
                horaParaExecucao = horaParaExecucao.AddDays(1);

            return horaParaExecucao;
        }

        private Task StartEmpresasProcess(string login, string senha, string fileDefaultPath,
            KeyValuePair<Parametro, string> date)
        {
            DateTime fromDate;
            if (
                !DateTime.TryParseExact(date.Value, "dd/MM/yyyy", CultureInfo.GetCultureInfo("pt-br").DateTimeFormat,
                    DateTimeStyles.None, out fromDate))
            {
                fromDate = DateTime.Now.Date.AddDays(-1);
            }

            login = login ?? string.Empty;
            senha = senha ?? string.Empty;
            fileDefaultPath = string.IsNullOrWhiteSpace(fileDefaultPath) ? "empresas_" : fileDefaultPath;

            var scheduleTime = DateTime.Now;

            var t1 = ProcessEmpresas(login, senha, fileDefaultPath, fromDate);

            t1.ContinueWith(t =>
            {
                if ((t.Result ?? string.Empty).IndexOf("OK", StringComparison.OrdinalIgnoreCase) > -1)
                {
                    ProcessCompletedWithSuccess(login, senha, fileDefaultPath, date, fromDate, scheduleTime);
                }
                else
                {
                    LocalLog(new InvalidOperationException(string.Format("ProcessResult='{0}'", t1.Result)),
                        "By Process Result (Empresas)");
                }
            }, TaskContinuationOptions.OnlyOnRanToCompletion);

            t1.ContinueWith(t =>
            {
                if (t.Exception == null)
                    return;

                LocalLog(t.Exception, "by Task Process (Empresas)");
            }, TaskContinuationOptions.OnlyOnFaulted);

            return t1;
        }

        private Task<string> ProcessEmpresas(string login, string senha, string file, DateTime fromDate)
        {
            var settings = GetExporterSettings(fromDate);

            //var exporter = new AllinCurriculoDefaultExporter(settings);

            var exporter = new ExporterEmpresaDefault(settings);

            var composition = GetLazyFtpStreamBuilder(login, senha, file);

            var lazyStream = composition.Key;
            var toDispose = composition.Value;

            var tR = exporter.Process(lazyStream, CancellationToken.None);

            tR.ContinueWith(t =>
            {
                try
                {
                    if (lazyStream.IsValueCreated)
                    {
                        lazyStream.Value.Close();
                        lazyStream.Value.Dispose();
                    }
                }
                catch
                {
                }

                try
                {
                    toDispose.Dispose();
                }
                catch
                {
                }
            });
            return tR;
        }

        private static MainExporterSettings GetExporterSettings(DateTime fromDate)
        {
            var settings = new MainExporterSettings();
            settings.StartSettings.FromDate = fromDate;
            settings.StartSettings.WriterHeader = true;
            settings.StartSettings.BatchSize = 2000;
            settings.StartSettings.BufferSize = 400;
            return settings;
        }

        private Task StartCurriculoProcess(string login, string senha, string fileDefaultPath,
            KeyValuePair<Parametro, string> date)
        {
            DateTime fromDate;
            if (
                !DateTime.TryParseExact(date.Value, "dd/MM/yyyy", CultureInfo.GetCultureInfo("pt-br").DateTimeFormat,
                    DateTimeStyles.None, out fromDate))
            {
                fromDate = DateTime.Now.Date.AddDays(-1);
            }

            login = login ?? string.Empty;
            senha = senha ?? string.Empty;
            fileDefaultPath = string.IsNullOrWhiteSpace(fileDefaultPath) ? "prd_" : fileDefaultPath;

            var scheduleTime = DateTime.Now;

            var t1 = ProcessCurriculo(login, senha, fileDefaultPath, fromDate);

            t1.ContinueWith(t =>
            {
                if ((t.Result ?? string.Empty).IndexOf("OK", StringComparison.OrdinalIgnoreCase) > -1)
                {
                    ProcessCompletedWithSuccess(login, senha, fileDefaultPath, date, fromDate, scheduleTime);
                }
                else
                {
                    LocalLog(new InvalidOperationException(string.Format("ProcessResult='{0}'", t1.Result)),
                        "By Process Result (Currículos)");
                }
            }, TaskContinuationOptions.OnlyOnRanToCompletion);

            t1.ContinueWith(t =>
            {
                if (t.Exception == null)
                    return;

                LocalLog(t.Exception, "by Task Process (Currículos)");
            }, TaskContinuationOptions.OnlyOnFaulted);

            return t1;
        }

        private void ProcessCompletedWithSuccess(string login, string senha, string fileDefaultPath,
            KeyValuePair<Parametro, string> date, DateTime fromDate, DateTime scheduledTime)
        {
            var newDate = scheduledTime.Date.ToString("dd/MM/yyyy");
            if (date.Key.ValorParametro != newDate)
            {
                date.Key.ValorParametro = newDate;
                date.Key.Save();
            }
            using (var conn = new FtpClient())
            {
                conn.Host = FtpHost;
                conn.Credentials = new NetworkCredential(login, senha);
                conn.Connect();
                RemoveOldFiles(conn, fileDefaultPath, scheduledTime);
            }
        }

        private Task<string> ProcessCurriculo(string login, string senha, string file, DateTime fromDate)
        {
            var settings = GetExporterSettings(fromDate);
            var exporter = new AllinCurriculoDefaultExporter(settings);

            var composition = GetLazyFtpStreamBuilder(login, senha, file);

            var lazyStream = composition.Key;
            var toDispose = composition.Value;

            var tR = exporter.Process(lazyStream, CancellationToken.None);

            tR.ContinueWith(t =>
            {
                try
                {
                    if (lazyStream.IsValueCreated)
                    {
                        lazyStream.Value.Close();
                        lazyStream.Value.Dispose();
                    }
                }
                catch
                {
                }

                try
                {
                    toDispose.Dispose();
                }
                catch
                {
                }
            });
            return tR;
        }

        private KeyValuePair<Lazy<Stream>, CompositeDisposable> GetLazyFtpStreamBuilder(string login, string senha,
            string file)
        {
            var disposer = new CompositeDisposable();

            var lazyStream = new Lazy<Stream>(() =>
            {
                FtpClient conn = null;
                try
                {
                    conn = new FtpClient();

                    conn.Host = FtpHost;
                    conn.Credentials = new NetworkCredential(login, senha);
                    conn.Connect();

                    Stream stream = null;
                    try
                    {
                        stream = conn.OpenWrite(file.Replace("data", DateTime.Now.ToString("ddMMyyyy")));
                    }
                    catch
                    {
                        if (stream != null)
                            stream.Dispose();

                        throw;
                    }

                    disposer.Add(stream);
                    disposer.Add(conn);
                    return stream;
                }
                catch
                {
                    if (conn != null)
                        conn.Dispose();

                    throw;
                }
            });
            return new KeyValuePair<Lazy<Stream>, CompositeDisposable>(lazyStream, disposer);
        }

        private static void LocalLog(Exception exp, string details)
        {
            GerenciadorException.GravarExcecao(exp, details + " (AllIn)");
            EmailSenderFactory.Create(TipoEnviadorEmail.Fila)
                .Enviar("appBNE - Erro: Serviço de sincronização da AllIn (" + details + ")", exp.DumpExInternal(), null,
                    "gieyson@bne.com.br", "gieyson@bne.com.br");
        }

        #region [ Properties ]
        public string FtpHost
        {
            get { return Settings.Default.AllInFtpHost; }
        }

        public double DaysLimitToRemoveOldFiles
        {
            get { return Settings.Default.AllInDiasLimiteHistorico; }
        }

        private EventLogWriter EventLogWriter { get; set; }
        #endregion

        #region [ Constructor ]
        static AllInEmailSincronizacaoLista()
        {
            Scheduler.Default.Catch<Exception>(DefaultSchedulerLog);
        }

        private static bool DefaultSchedulerLog(Exception exp)
        {
            LocalLog(exp, "by Scheduler.Default");
            return true;
        }

        public AllInEmailSincronizacaoLista()
        {
            InitializeComponent();
            EventLogWriter = new EventLogWriter(Settings.Default.LogName, GetType().Name);
        }
        #endregion

        #region [ Protected ]
        protected override void OnStart(string[] args)
        {
            var firstRunning = true;
            _subscription = Scheduler.Default.Schedule(GetNextExecution(), next =>
            {
                try
                {
                    DoCandidato(firstRunning).ContinueWith(t =>
                    {
                        if (t.Status == TaskStatus.RanToCompletion)
                            EventLogWriter.LogEvent(
                                string.Format("Exportação de currículos da plataforma AllIn Finalizado: {0}.",
                                    DateTime.Now), EventLogEntryType.Information, Event.InicioExecucao);
                        else
                            EventLogWriter.LogEvent(
                                string.Format(
                                    "Exportação de currículos da plataforma AllIn ERRO ao completar ({0}): {1}.",
                                    t.Status, DateTime.Now), EventLogEntryType.Information, Event.InicioExecucao);
                    })
                        .ContinueWith(t => { return DoEmpresa(firstRunning); }).Unwrap()
                        .ContinueWith(t =>
                        {
                            if (t.Status == TaskStatus.RanToCompletion)
                                EventLogWriter.LogEvent(
                                    string.Format("Exportação de empresas da plataforma AllIn Finalizado: {0}.",
                                        DateTime.Now), EventLogEntryType.Information, Event.InicioExecucao);
                            else
                                EventLogWriter.LogEvent(
                                    string.Format(
                                        "Exportação de empresas da plataforma AllIn ERRO ao completar ({0}): {1}.",
                                        t.Status, DateTime.Now), EventLogEntryType.Information, Event.InicioExecucao);
                        });


                    firstRunning = false;
                }
                finally
                {
                    next(GetNextExecution());
                }
            });
        }


        protected override void OnStop()
        {
            var subs = _subscription;
            if (subs != null)
            {
                subs.Dispose();
            }
        }
        #endregion

        #region [ Public ]
        public Task DoCandidato(bool firstRunning)
        {
            EventLogWriter.LogEvent(
                string.Format("Exportação de e-mails e currículos da plataforma AllIn Iniciado: {0}.", DateTime.Now),
                EventLogEntryType.Information, Event.InicioExecucao);

            var login = new Parametro((int) BLL.Enumeradores.Parametro.AllinFtpBneLogin);
            var senha = new Parametro((int) BLL.Enumeradores.Parametro.AllinFtpBneSenha);
            var fileName = new Parametro((int) BLL.Enumeradores.Parametro.AllinAtualizacaoArquivoNome);
            var fromDate = new Parametro((int) BLL.Enumeradores.Parametro.AllinAtualizacaoCurriculosApartirDe);

            Action<Func<bool>, string> handleParameters = (evaluation, desc) =>
            {
                if (!evaluation())
                {
                    throw new InvalidOperationException(desc + " não disponível no banco de dados.");
                }
            };

            handleParameters(() => login.CompleteObject(),
                string.Format("login ({0}={1})", BLL.Enumeradores.Parametro.AllinFtpBneLogin,
                    (int) BLL.Enumeradores.Parametro.AllinFtpBneLogin));
            handleParameters(() => senha.CompleteObject(),
                string.Format("senha ({0}={1})", BLL.Enumeradores.Parametro.AllinFtpBneSenha,
                    (int) BLL.Enumeradores.Parametro.AllinFtpBneSenha));
            handleParameters(() => fileName.CompleteObject(),
                string.Format("fileName ({0}={1})", BLL.Enumeradores.Parametro.AllinAtualizacaoArquivoNome,
                    (int) BLL.Enumeradores.Parametro.AllinAtualizacaoArquivoNome));
            handleParameters(() => fromDate.CompleteObject(),
                string.Format("fromDate ({0}={1})", BLL.Enumeradores.Parametro.AllinAtualizacaoCurriculosApartirDe,
                    (int) BLL.Enumeradores.Parametro.AllinAtualizacaoCurriculosApartirDe));

            return StartCurriculoProcess(login.ValorParametro, senha.ValorParametro, fileName.ValorParametro,
                new KeyValuePair<Parametro, string>(fromDate, fromDate.ValorParametro));
        }


        public Task DoEmpresa(bool firstRunning)
        {
            EventLogWriter.LogEvent(
                string.Format("Exportação de e-mails para empresas usando plataforma AllIn Iniciado: {0}.", DateTime.Now),
                EventLogEntryType.Information, Event.InicioExecucao);

            var login = new Parametro((int) BLL.Enumeradores.Parametro.AllinFtpBneLogin);
            var senha = new Parametro((int) BLL.Enumeradores.Parametro.AllinFtpBneSenha);
            var fileName = new Parametro((int) BLL.Enumeradores.Parametro.AllinEmpresasArquivoNome);
            var fromDate = new Parametro((int) BLL.Enumeradores.Parametro.AllinAtualizacaoEmpresasApartirDe);

            Action<Func<bool>, string> handleParameters = (evaluation, desc) =>
            {
                if (!evaluation())
                {
                    throw new InvalidOperationException(desc + " não disponível no banco de dados.");
                }
            };

            handleParameters(() => login.CompleteObject(),
                string.Format("login ({0}={1})", BLL.Enumeradores.Parametro.AllinFtpBneLogin,
                    (int) BLL.Enumeradores.Parametro.AllinFtpBneLogin));
            handleParameters(() => senha.CompleteObject(),
                string.Format("senha ({0}={1})", BLL.Enumeradores.Parametro.AllinFtpBneSenha,
                    (int) BLL.Enumeradores.Parametro.AllinFtpBneSenha));
            handleParameters(() => fileName.CompleteObject(),
                string.Format("fileName ({0}={1})", BLL.Enumeradores.Parametro.AllinEmpresasArquivoNome,
                    (int) BLL.Enumeradores.Parametro.AllinEmpresasArquivoNome));
            handleParameters(() => fromDate.CompleteObject(),
                string.Format("fromDate ({0}={1})", BLL.Enumeradores.Parametro.AllinAtualizacaoEmpresasApartirDe,
                    (int) BLL.Enumeradores.Parametro.AllinAtualizacaoEmpresasApartirDe));

            return StartEmpresasProcess(login.ValorParametro, senha.ValorParametro, fileName.ValorParametro,
                new KeyValuePair<Parametro, string>(fromDate, fromDate.ValorParametro));
        }

        public void RemoveOldFiles(FtpClient conn, string file, DateTime scheduledTime)
        {
            var allItems = conn.GetListing();

            var index = file.IndexOf("data", StringComparison.OrdinalIgnoreCase);
            string toFind;
            if (index > 0)
            {
                toFind = file.Substring(0, index);
            }
            else
            {
                toFind = file.Replace(".csv", "");
            }

            var filterItems =
                allItems.Where(
                    a =>
                        ((a.FullName ?? string.Empty).Split('/').LastOrDefault() ?? string.Empty).StartsWith(toFind,
                            StringComparison.OrdinalIgnoreCase));
            foreach (var item in filterItems)
            {
                var splitName = (item.FullName ?? string.Empty).Split('_');
                splitName = splitName.SelectMany(a => a.Split('.')).ToArray();

                var processDate = default(DateTime);
                if (
                    splitName.Any(
                        a =>
                            DateTime.TryParseExact((a ?? string.Empty).Trim(), "ddMMyyyy",
                                CultureInfo.GetCultureInfo("pt-br").DateTimeFormat, DateTimeStyles.None, out processDate)))
                {
                    var daysToRemove = Math.Abs(DaysLimitToRemoveOldFiles);
                    var reachedDate = scheduledTime.Date.AddDays(-daysToRemove);

                    if (processDate > reachedDate)
                        continue;

                    conn.DeleteFile(item.FullName);
                }
            }
        }
        #endregion
    }

    internal static class DumpExtension
    {
        public static string DumpExInternal(this Exception ex)
        {
            if (ex == null)
                return "Exception is null.";

            var allMsg = new StringBuilder();
            var count = 0;
            var genericException = ex;

            var avoidsStackOverflow = new List<Exception>();
            try
            {
                do
                {
                    var partialMsg = new StringBuilder();
                    count++;
                    partialMsg.Append("[");
                    partialMsg.Append(count);
                    partialMsg.Append("]");

                    if (count > 1)
                        partialMsg.Append(" InnerException Type: ");
                    else
                        partialMsg.Append(" Exception Type: ");

                    partialMsg.Append(genericException.GetType());

                    if (!string.IsNullOrEmpty(genericException.Message))
                    {
                        partialMsg.AppendLine();
                        partialMsg.AppendLine();
                        partialMsg.Append("Message: ");
                        partialMsg.Append(genericException.Message);
                    }

                    if (!string.IsNullOrEmpty(genericException.Source))
                    {
                        partialMsg.AppendLine();
                        partialMsg.AppendLine();
                        partialMsg.Append("Source: ");
                        partialMsg.Append(genericException.Source);
                    }

                    if (!string.IsNullOrEmpty(genericException.StackTrace))
                    {
                        partialMsg.AppendLine();
                        partialMsg.AppendLine();
                        partialMsg.Append("StackTrace: ");
                        partialMsg.Append(genericException.StackTrace);
                    }


                    if (genericException.TargetSite != null)
                    {
                        partialMsg.AppendLine();
                        partialMsg.AppendLine();
                        partialMsg.Append("TargetSite: name=");
                        partialMsg.Append(genericException.TargetSite.Name);

                        partialMsg.Append(", type=");
                        partialMsg.Append(genericException.TargetSite.ReflectedType);

                        partialMsg.Append(", assembly=");
                        partialMsg.Append(genericException.TargetSite.ReflectedType.Assembly);
                    }

                    if (!string.IsNullOrEmpty(genericException.HelpLink))
                    {
                        partialMsg.AppendLine();
                        partialMsg.AppendLine();
                        partialMsg.Append("HelpLink: ");
                        partialMsg.Append(genericException.HelpLink);
                    }

                    if (genericException.Data != null && genericException.Data.Count > 0)
                    {
                        partialMsg.AppendLine();
                        partialMsg.AppendLine();
                        partialMsg.Append("Data [");
                        partialMsg.Append(genericException.Data.Count);
                        partialMsg.Append("]: ");
                        partialMsg.AppendLine();

                        foreach (DictionaryEntry item in genericException.Data)
                        {
                            try
                            {
                                partialMsg.AppendFormat("Key='{0}' | Value='{1}'", item.Key, item.Value);
                                partialMsg.AppendLine();
                            }
                            catch
                            {
                            }
                        }
                    }

                    var exceptionType = genericException.GetType();
                    while (exceptionType != typeof (Exception) && exceptionType != null &&
                           (exceptionType.IsValueType || exceptionType.IsPrimitive))
                    {
                        var otherProperties =
                            exceptionType.GetProperties(BindingFlags.Public | BindingFlags.Instance |
                                                        BindingFlags.DeclaredOnly);
                        if (otherProperties.Length > 0)
                        {
                            var writeHeader = false;
                            foreach (var item in otherProperties)
                            {
                                if (!item.CanRead)
                                    continue;

                                var getMethod = item.GetGetMethod(false);
                                if (getMethod == null || getMethod.GetBaseDefinition() != getMethod)
                                    continue;

                                if (!writeHeader)
                                {
                                    partialMsg.AppendLine();
                                    partialMsg.AppendLine();
                                    partialMsg.Append("Other Information:");
                                    partialMsg.AppendLine();
                                    writeHeader = true;
                                }

                                partialMsg.AppendFormat("   PropertyName='{0}'", item.Name).AppendLine();
                                partialMsg.AppendFormat("   Value='{0}'", item.GetValue(genericException, null));
                                partialMsg.AppendLine();
                            }
                        }

                        exceptionType = exceptionType.BaseType;
                    }

                    if (count != 1)
                    {
                        partialMsg.AppendLine();
                        partialMsg.AppendLine();
                        partialMsg.AppendLine();
                    }
                    allMsg.Insert(0, partialMsg.ToString());

                    if (avoidsStackOverflow.Any(obj => obj == genericException))
                        break;

                    avoidsStackOverflow.Add(genericException);
                    genericException = genericException.InnerException;
                } while (genericException != null);
            }
            catch
            {
            }

            return allMsg.ToString();
        }
    }
}