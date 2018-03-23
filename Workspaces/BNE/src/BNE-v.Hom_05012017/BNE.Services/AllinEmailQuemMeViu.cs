using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Concurrency;
using System.Threading.Tasks;
using AllInMail.Core;
using AllInMail.Helper;
using AllInTriggers;
using AllInTriggers.Model;
using BNE.BLL;
using BNE.BLL.Custom;
using BNE.BLL.DTO;
using BNE.BLL.Enumeradores;
using BNE.Services.Base.EventLog;
using BNE.Services.Properties;
using Curriculo = BNE.BLL.Curriculo;
using Parametro = BNE.BLL.Parametro;
using TipoGatilho = BNE.BLL.TipoGatilho;

namespace BNE.Services
{
    public partial class AllinEmailQuemMeViu : BaseService
    {
        private IDisposable _subscription;

        public AllinEmailQuemMeViu()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            var firstRunning = true;
            _subscription = Scheduler.Default.Schedule(GetNextExecution(), next =>
            {
                try
                {
                    DoAsync(firstRunning).ContinueWith(t =>
                    {
                        if (t.Status == TaskStatus.RanToCompletion)
                            EventLogWriter.LogEvent(
                                string.Format("Quem Me Viu para a plataforma AllIn Finalizado: {0}.", DateTime.Now),
                                EventLogEntryType.Information, Event.InicioExecucao);
                        else
                            EventLogWriter.LogEvent(
                                string.Format("Quem Me Viu para a plataforma AllIn ERRO ao completar ({0}): {1}.",
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

        public Task DoAsync(bool firstRunning)
        {
            return Task.Factory.StartNew(() => { Do(firstRunning); });
        }

        public void Do(bool firstRunning)
        {
            var limit = Math.Abs(Settings.Default.AllInQuemMeViuDiasDeHistoricoQuemMeViu);

            var result = GetRelatorioQuantidadePorCurriculo(limit);

            var validation =
                new Func<SqlTransaction, bool>(
                    tr => TipoGatilho.GatilhoAtivo((int) BLL.Enumeradores.TipoGatilho.QuemMeViuSemanalNaoVip, tr));

            var lifeCycle = AllinCicloVida.CarregarPorGatilho(BLL.Enumeradores.TipoGatilho.QuemMeViuSemanalNaoVip);
            var transacional = AllinTransacional.CarregarPorGatilho(
                BLL.Enumeradores.TipoGatilho.QuemMeViuSemanalNaoVip);

            var con = new Lazy<SqlConnection>(() => CreateDataBaseOpenCon());
            var sql =
                new Func<SqlConnection, SqlTransaction>(arg => CreateDataBaseTrans(arg, IsolationLevel.ReadUncommitted));

            var loginAllin = new Lazy<string>(() => AllInTransaction.LoginTransactionCall().Execute());
            var conv = new AllInCurriculoDefConverter();

            using (var options = new ProcessQmMeViuOptions(validation, lifeCycle, transacional, loginAllin, con, sql))
            {
                if (!options.IsValid)
                    return;

                if (!options.LifeCycleAllIn.Any() && !options.TransactionAllIn.Any())
                    return;

                var agg = new List<Exception>();

#if DEBUG
                foreach (var item in result.Take(3))
#else
                foreach (var item in result)
#endif
                {
                    if (agg.Count > 10)
                        break;

                    try
                    {
                        Process(options, conv, item);
                    }
                    catch (Exception ex)
                    {
                        agg.Add(ex);
                    }
                }

                if (agg.Count > 0)
                {
                    throw new AggregateException(agg);
                }
            }
        }

        protected virtual IEnumerable<CurriculoQuemMeViu.RelatorioQuemMeViuModel> GetRelatorioQuantidadePorCurriculo(
            int limit)
        {
            var result = CurriculoQuemMeViu.RelatorioQuantidadePorCurriculo(DateTime.Now.Date.AddDays(-limit), 1);
            return result;
        }

        private static SqlConnection CreateDataBaseOpenCon()
        {
            var con = new SqlConnection(DataAccessLayer.CONN_STRING);
            con.Open();
            return con;
        }

        private static SqlTransaction CreateDataBaseTrans(SqlConnection con,
            IsolationLevel isolation = IsolationLevel.ReadCommitted)
        {
            var trans = con.BeginTransaction(isolation);
            return trans;
        }

        private void Process(ProcessQmMeViuOptions options, AllInCurriculoDefConverter conv,
            CurriculoQuemMeViu.RelatorioQuemMeViuModel data)
        {
            var bllModel = Curriculo.CarregarCurriculoExportacaoAllIn(data.IdCurriculo, options.DbTransaction);
            if (bllModel == null)
                return;

#if DEBUG
            //bllModel.Email = "nathankinapp@bne.com.br";
            bllModel.Email = "nkinapp@gmail.com";
#endif
            var allinModel = ModelTranslator.TranslateToAllInCurriculoModel(bllModel);

            var resValue = conv.Parse(allinModel);
            var defProperties = conv.GetDefiniedFields();
            var defValues = resValue.Split(';');
            defValues = defValues.Select(a => a.Length > 100
                ? new string(a.Take(98).Concat(new[] {'.', '.'}).ToArray())
                : a)
                .ToArray();

            var pairs =
                defProperties.Select((a, index) => new KeyValuePair<string, string>(a, defValues[index])).ToArray();

            var hash = GetLoginHash(bllModel);
            var dominio = GetTargetDomainToLogin();
            var urlLogin = Rota.RecuperarURLRota(RouteCollection.LogarAutomatico);

            var customPairs = new[]
            {
                new KeyValuePair<string, string>("QtdX", data.Total.ToString()),
                new KeyValuePair<string, string>("hash", hash),
                new KeyValuePair<string, string>("dominio", dominio.EndsWith("/") ? dominio : dominio + "/"),
                new KeyValuePair<string, string>("path", urlLogin.Replace("{HashAcesso}", string.Empty))
            };

            var err = new List<Exception>();

            foreach (var item in options.LifeCycleAllIn)
            {
                var obj = new NotificaCicloDeVidaAllIn();
                obj.AceitaRepeticao = item.FlagAceitaRepeticao;
                obj.IdentificadorAllIn = item.IdentificadorCicloAllin;
                obj.Evento = item.DescricaoEvento;
                obj.CamposComValores =
                    pairs.Concat(customPairs)
                        .Concat(new[] {new KeyValuePair<string, string>("utm", item.DescricaoGoogleUtm)})
                        .ToArray();
                obj.EmailEnvio = bllModel.Email;
#if DEBUG
                //obj.EmailEnvio = "nathankinapp@bne.com.br";
                obj.EmailEnvio = "nkinapp@gmail.com";
#endif


                var res = AllInPainel.NotificarCicloDeVidaCall(obj).Execute();

                if (res == null || res.IndexOf("ok", StringComparison.OrdinalIgnoreCase) == -1)
                {
                    err.Add(
                        new InvalidOperationException(string.Format("Evento='{0}'|Result='{1}'", obj.EmailEnvio, res)));
                }
            }

            foreach (var item in options.TransactionAllIn)
            {
                var obj = new EnviaEmailTransacaoAllIn();
                obj.Assunto = item.DescricaoAssunto;
                obj.Campos = defProperties.Concat(customPairs.Select(a => a.Key)).Concat(new[] {"utm"}).ToArray();
                obj.DataHoraEnvio = CalcularHoraEnvio(item.HoraDisparo);
                obj.EmailEnvio = bllModel.Email;
                obj.EmailRemente = item.EmailRemetente;
                obj.EmailResposta = item.EmailResposta;
                obj.HtmAllInlId = item.IdentificadorHtmlAllin;
                obj.NomeRemente = item.NomeRemetente;
                obj.Valores =
                    defValues.Concat(customPairs.Select(a => a.Value)).Concat(new[] {item.DescricaoGoogleUtm}).ToArray();

                //todo testar e validar resultado se for utilizar
                AllInTransaction.EnviarEmailCall(options.LoginTransactionAllInKey, obj).Execute();
            }

            if (err.Count > 0)
            {
                if (err.Count == 1)
                    throw err[0];

                throw new AggregateException(err);
            }
        }

        private DateTime CalcularHoraEnvio(TimeSpan? nullable)
        {
            if (!nullable.HasValue)
            {
                return DateTime.Now;
            }

            var cur = DateTime.Now;
            var target = new DateTime(cur.Year, cur.Month, cur.Day, nullable.Value.Hours, nullable.Value.Minutes, 0);

            if (cur == target)
            {
                return target;
            }
            if (cur < target)
            {
                return target;
            }

            return target.AddDays(1);
        }

        private string GetTargetDomainToLogin()
        {
            var url = Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.URLAmbiente);
            url = (url ?? string.Empty).Trim();

            if (url.IndexOf("www.", StringComparison.OrdinalIgnoreCase) > -1)
                return url;

            if (url.StartsWith(@"http://", StringComparison.OrdinalIgnoreCase))
            {
                var domainCout = url.Split('.');

                if (domainCout.Length >= 2 && domainCout.Length < 4)
                    return url.Replace(@"http://", @"http://wwww.");

                return url;
            }

            var subDomainCount = url.Split('.');
            if (subDomainCount.Length >= 2 && subDomainCount.Length < 4)
                return @"http://www." + url;

            return @"http://" + url;
        }

        private string GetLoginHash(AllInCurriculo bllModel)
        {
            var pf = new PessoaFisica(bllModel.IdPessoaFisica);
            pf.CPF = bllModel.NumeroCPF;
            pf.DataNascimento = bllModel.DataNascimento;

            var hash = LoginAutomatico.GerarHashAcessoLogin(pf);
            return hash;
        }

        private DateTime GetNextExecution()
        {
            var now = DateTime.Now;
            var horaMinuto = Settings.Default.AllInQuemMeViuHoraExecucao.Split(':');

            var horaParaExecucao = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                Convert.ToInt32(horaMinuto[0]), Convert.ToInt32(horaMinuto[1]), 0);

            if (horaParaExecucao.Subtract(now).Ticks < 0)
                horaParaExecucao = horaParaExecucao.AddDays(1);

            while (!Settings.Default.AllInQuemMeViuDiasDaSemanaParaExecutar.HasFlag(horaParaExecucao.DayOfWeek))
                horaParaExecucao = horaParaExecucao.AddDays(1);

            return horaParaExecucao;
        }

        protected override void OnStop()
        {
            var subs = _subscription;
            if (subs != null)
            {
                subs.Dispose();
            }
        }

        private sealed class ProcessQmMeViuOptions : IDisposable
        {
            private readonly Lazy<SqlConnection> _con;
            private readonly Lazy<string> _loginTransaction;

            private readonly Lazy<SqlTransaction> _trans;
            private readonly Lazy<bool> _validation;

            public IBuffer<AllinCicloVida> LifeCycleAllIn { get; set; }
            public IBuffer<AllinTransacional> TransactionAllIn { get; set; }

            public string LoginTransactionAllInKey
            {
                get { return _loginTransaction.Value; }
            }

            public SqlTransaction DbTransaction
            {
                get { return _trans.Value; }
            }

            public bool IsValid
            {
                get { return _validation.Value; }
            }

            public ProcessQmMeViuOptions(Func<SqlTransaction, bool> validation,
                IEnumerable<AllinCicloVida> lifeCycleModel, IEnumerable<AllinTransacional> transactionModel,
                Lazy<string> transactionAllInLoginKey, Lazy<SqlConnection> con,
                Func<SqlConnection, SqlTransaction> trans)
            {
                _validation = new Lazy<bool>(() => validation(DbTransaction));
                LifeCycleAllIn = (lifeCycleModel ?? Enumerable.Empty<AllinCicloVida>()).Memoize();
                TransactionAllIn = (transactionModel ?? Enumerable.Empty<AllinTransacional>()).Memoize();
                _con = con;
                _trans = new Lazy<SqlTransaction>(() => trans(_con.Value));
                _loginTransaction = transactionAllInLoginKey ?? new Lazy<string>(() => string.Empty);
            }


            public void Dispose()
            {
                LifeCycleAllIn.Dispose();
                TransactionAllIn.Dispose();

                if (_trans.IsValueCreated)
                {
                    _trans.Value.Dispose();
                }

                if (_con.IsValueCreated)
                {
                    _con.Value.Dispose();
                }
            }
        }
    }
}