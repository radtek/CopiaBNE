using AllInMail.Core;
using BNE.Services.Code;
using BNE.Services.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Concurrency;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace BNE.Services
{
    public partial class AllinEmailQuemMeViu : ServiceBase
    {
        private sealed class ProcessQmMeViuOptions : IDisposable
        {
            public ProcessQmMeViuOptions(Func<SqlTransaction, bool> validation, IEnumerable<BLL.AllinCicloVida> lifeCycleModel, IEnumerable<BLL.AllinTransacional> transactionModel, Lazy<string> transactionAllInLoginKey, Lazy<SqlConnection> con, Func<SqlConnection, SqlTransaction> trans)
            {
                this._validation = new Lazy<bool>(() => validation(DbTransaction));
                this.LifeCycleAllIn = (lifeCycleModel ?? Enumerable.Empty<BLL.AllinCicloVida>()).Memoize();
                this.TransactionAllIn = (transactionModel ?? Enumerable.Empty<BLL.AllinTransacional>()).Memoize();
                this._con = con;
                this._trans = new Lazy<SqlTransaction>(() => trans(_con.Value));
                this._loginTransaction = transactionAllInLoginKey ?? new Lazy<string>(() => string.Empty);
            }

            public IBuffer<BLL.AllinCicloVida> LifeCycleAllIn { get; set; }
            public IBuffer<BLL.AllinTransacional> TransactionAllIn { get; set; }

            private Lazy<SqlTransaction> _trans;
            private Lazy<SqlConnection> _con;
            private Lazy<string> _loginTransaction;
            private Lazy<bool> _validation;


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
                get
                {
                    return _validation.Value;
                }
            }
        }

        public const string EventSourceName = "AllinEmailQuemMeViu";

        private IDisposable _subscription;
        public AllinEmailQuemMeViu()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            bool firstRunning = true;
            _subscription = Scheduler.Default.Schedule(GetNextExecution(), (next) =>
            {
                try
                {
                    DoAsync(firstRunning).ContinueWith(t =>
                    {
                        if (t.Status == TaskStatus.RanToCompletion)
                            EventLogWriter.LogEvent(EventSourceName, String.Format("Quem Me Viu para a plataforma AllIn Finalizado: {0}.", DateTime.Now), EventLogEntryType.Information, (int)EventID.InicioExecucao);
                        else
                            EventLogWriter.LogEvent(EventSourceName, String.Format("Quem Me Viu para a plataforma AllIn ERRO ao completar ({0}): {1}.", t.Status, DateTime.Now), EventLogEntryType.Information, (int)EventID.InicioExecucao);
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
            return Task.Factory.StartNew(() =>
                {
                    Do(firstRunning);
                });
        }

        public void Do(bool firstRunning)
        {
            var limit = Math.Abs(Settings.Default.AllInQuemMeViuDiasDeHistoricoQuemMeViu);

            var result = GetRelatorioQuantidadePorCurriculo(limit);

            var validation = new Func<SqlTransaction, bool>(tr => BLL.TipoGatilho.GatilhoAtivo((int)BNE.BLL.Enumeradores.TipoGatilho.QuemMeViuSemanalNaoVip, tr));

            var lifeCycle = BLL.AllinCicloVida.CarregarPorGatilho(BLL.Enumeradores.TipoGatilho.QuemMeViuSemanalNaoVip, null);
            var transacional = BLL.AllinTransacional.CarregarPorGatilho(BLL.Enumeradores.TipoGatilho.QuemMeViuSemanalNaoVip, null);

            var con = new Lazy<SqlConnection>(() => CreateDataBaseOpenCon());
            var sql = new Func<SqlConnection, SqlTransaction>(arg => CreateDataBaseTrans(arg, IsolationLevel.ReadUncommitted));

            var loginAllin = new Lazy<string>(() => AllInTriggers.AllInTransaction.LoginTransactionCall().Execute());
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

        protected virtual IEnumerable<BLL.CurriculoQuemMeViu.RelatorioQuemMeViuModel> GetRelatorioQuantidadePorCurriculo(int limit)
        {
            var result = BNE.BLL.CurriculoQuemMeViu.RelatorioQuantidadePorCurriculo(DateTime.Now.Date.AddDays(-limit), 1);
            return result;
        }

        private static SqlConnection CreateDataBaseOpenCon()
        {
            var con = new SqlConnection(BNE.BLL.DataAccessLayer.CONN_STRING);
            con.Open();
            return con;
        }

        private static SqlTransaction CreateDataBaseTrans(SqlConnection con, IsolationLevel isolation = IsolationLevel.ReadCommitted)
        {
            var trans = con.BeginTransaction(isolation);
            return trans;
        }

        private void Process(ProcessQmMeViuOptions options, AllInCurriculoDefConverter conv, BLL.CurriculoQuemMeViu.RelatorioQuemMeViuModel data)
        {
            var bllModel = BLL.Curriculo.CarregarCurriculoExportacaoAllIn(data.IdCurriculo, options.DbTransaction);
            if (bllModel == null)
                return;

#if DEBUG
            //bllModel.Email = "nathankinapp@bne.com.br";
            bllModel.Email = "nkinapp@gmail.com";
#endif
            var allinModel = AllInMail.Helper.ModelTranslator.TranslateToAllInCurriculoModel(bllModel);

            var resValue = conv.Parse(allinModel);
            var defProperties = conv.GetDefiniedFields();
            var defValues = resValue.Split(';');
            defValues = defValues.Select(a => a.Length > 100 ?
                                        new string(a.Take(98).Concat(new[] { '.', '.' }).ToArray()) : a)
                                    .ToArray();

            var pairs = defProperties.Select((a, index) => new KeyValuePair<string, string>(a, defValues[index])).ToArray();

            var hash = GetLoginHash(bllModel);
            var dominio = GetTargetDomainToLogin();
            var urlLogin = BLL.Rota.RecuperarURLRota(BLL.Enumeradores.RouteCollection.LogarAutomatico);

            var customPairs = new[] { 
                                        new KeyValuePair<string,string>("QtdX", data.Total.ToString()),
                                        new KeyValuePair<string, string>("hash", hash),
                                        new KeyValuePair<string, string>("dominio", dominio.EndsWith("/") ? dominio : dominio + "/"),
                                        new KeyValuePair<string, string>("path", urlLogin.Replace("{HashAcesso}", string.Empty)),
                                    };

            var err = new List<Exception>();

            foreach (var item in options.LifeCycleAllIn)
            {
                var obj = new AllInTriggers.Model.NotificaCicloDeVidaAllIn();
                obj.AceitaRepeticao = item.FlagAceitaRepeticao;
                obj.IdentificadorAllIn = item.IdentificadorCicloAllin.ToString();
                obj.Evento = item.DescricaoEvento;
                obj.CamposComValores = pairs.Concat(customPairs).Concat(new[] { new KeyValuePair<string, string>("utm", item.DescricaoGoogleUtm) }).ToArray();
                obj.EmailEnvio = bllModel.Email;
#if DEBUG
                //obj.EmailEnvio = "nathankinapp@bne.com.br";
                obj.EmailEnvio = "nkinapp@gmail.com";
#endif


                var res = AllInTriggers.AllInPainel.NotificarCicloDeVidaCall(obj).Execute();

                if (res == null || res.IndexOf("ok", StringComparison.OrdinalIgnoreCase) == -1)
                {
                    err.Add(new InvalidOperationException(string.Format("Evento='{0}'|Result='{1}'", obj.EmailEnvio, res)));
                }
            }

            foreach (var item in options.TransactionAllIn)
            {
                var obj = new AllInTriggers.Model.EnviaEmailTransacaoAllIn();
                obj.Assunto = item.DescricaoAssunto;
                obj.Campos = defProperties.Concat(customPairs.Select(a => a.Key)).Concat(new[] { "utm" }).ToArray();
                obj.DataHoraEnvio = CalcularHoraEnvio(item.HoraDisparo);
                obj.EmailEnvio = bllModel.Email;
                obj.EmailRemente = item.EmailRemetente;
                obj.EmailResposta = item.EmailResposta;
                obj.HtmAllInlId = item.IdentificadorHtmlAllin;
                obj.NomeRemente = item.NomeRemetente;
                obj.Valores = defValues.Concat(customPairs.Select(a => a.Value)).Concat(new[] { item.DescricaoGoogleUtm }).ToArray();

                //todo testar e validar resultado se for utilizar
                AllInTriggers.AllInTransaction.EnviarEmailCall(options.LoginTransactionAllInKey, obj).Execute();
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
            else if (cur < target)
            {
                return target;
            }

            return target.AddDays(1);
        }

        private string GetTargetDomainToLogin()
        {
            var url = BLL.Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.URLAmbiente);
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

        private string GetLoginHash(BLL.DTO.AllInCurriculo bllModel)
        {
            var pf = new BLL.PessoaFisica(bllModel.IdPessoaFisica);
            pf.CPF = bllModel.NumeroCPF;
            pf.DataNascimento = bllModel.DataNascimento;

            var hash = BNE.BLL.Custom.LoginAutomatico.GerarHashAcessoLogin(pf);
            return hash;
        }

        private DateTime GetNextExecution()
        {
            var now = DateTime.Now;
            string[] horaMinuto = Settings.Default.AllInQuemMeViuHoraExecucao.Split(':');

            DateTime horaParaExecucao = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Convert.ToInt32(horaMinuto[0]), Convert.ToInt32(horaMinuto[1]), 0);

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
    }
}
