using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using AllInMail;
using AllInMail.Core;
using AllInMail.Helper;
using AllInTriggers;
using AllInTriggers.Base;
using AllInTriggers.Core;
using AllInTriggers.Helper;
using AllInTriggers.Model;
using BNE.BLL;
using BNE.BLL.Custom;
using BNE.BLL.DTO;
using BNE.BLL.Enumeradores;
using BNE.EL;
using BNE.Services.Plugins.PluginSaida.AllInOutput;
using Curriculo = BNE.BLL.Curriculo;
using Parametro = BNE.BLL.Enumeradores.Parametro;
using TipoGatilho = BNE.BLL.TipoGatilho;
using Vaga = BNE.BLL.Vaga;

namespace BNE.Services.Plugins.PluginsEntrada.AllInInput
{
    public class TriggerProcessor : IProcessor
    {
        public IDisposable CreateSubscriptions(IReactiveFlowInvoker invoker)
        {
            var disp = new CompositeDisposable();

            var mainSubs = CreateMainSubs(invoker);
            disp.Add(mainSubs);

            var registerSubs = CreateLifeCycleStandardTrigger(invoker);
            disp.Add(registerSubs);

            // todo (to do) create a standard trigger to TransactionEvent in Allin
            // disp.add(other)

            var searchVaga = CreateSearchTriggger(invoker);
            disp.Add(searchVaga);

            var inSx = invoker.Mediator.CreateIfNotExists<DynObj, int>(EventTitleToAllin.TriggerLogin, a => a.Maybe<int>("IdPessoaFisica").ValueOrDefault);
            var outSx = invoker.Mediator.CreateIfNotExists<DynObj, int>(EventTitleToAllin.TriggerCandidateResearchedJob, a => a.Maybe<int>("IdPessoaFisica").ValueOrDefault);
            var mainSx = invoker.Mediator.CreateIfNotExists<DynObj, int>(EventTitleToAllin.TriggerLogout, a => a.Maybe<int>("IdPessoaFisica").ValueOrDefault);

            var ignorable = CreateIgnorableSubs(invoker, "Async Multi Trigger", inSx.Broadcast().ObserveOn(invoker.Context).Select(a => a.OriginalEvent),
                                                                                outSx.Broadcast().ObserveOn(invoker.Context).Select(a => a.OriginalEvent),
                                                                                mainSx.Broadcast().ObserveOn(invoker.Context).Select(a => a.OriginalEvent));
            disp.Add(ignorable);
            return disp;
        }

        private IDisposable CreateSearchTriggger(IReactiveFlowInvoker invoker)
        {
            var inSx = invoker.Mediator.CreateIfNotExists<DynObj, int>(EventTitleToAllin.TriggerLogin, a => a.Maybe<int>("IdPessoaFisica").ValueOrDefault);
            var outSx = invoker.Mediator.CreateIfNotExists<DynObj, int>(EventTitleToAllin.TriggerLogout, a => a.Maybe<int>("IdPessoaFisica").ValueOrDefault);
            var mainSx = invoker.Mediator.CreateIfNotExists<DynObj, int>(EventTitleToAllin.TriggerCandidateResearchedJob, a => a.Maybe<int>("IdPessoaFisica").ValueOrDefault);

            var broadCastIn = inSx.Broadcast()
                                .ObserveOn(invoker.Context)
                                .Select(a =>
                                        new
                                        {
                                            EventData = a,
                                            Task = PermitidoEnviarEmailBaseadoEmPesquisasRealizadas(a)
                                        })
                                .DoWithFallBack(async a => await a.Task, (next, ex) => FallBackPlan(invoker, next.EventData.OriginalEvent, ex, false))
                                .WhereFallBack(a => a.Task.Result, a => ConditionBlockPlan(invoker, a.EventData.OriginalEvent, "Usuário já recebeu e-mail.", false))
                                .Select(a => a.EventData)
                                .Publish().RefCount();

            var groupSx = broadCastIn.GroupByUntil(a => a.OriginalEvent.Maybe<int>("IdPessoaFisica").ValueOrDefault, a =>
                {   //  Console.WriteLine("Create Group") 
                    return
                        a.Select(obj =>
                        { //  Console.WriteLine("Create Select of Logout");
                            var shareOutSx = outSx.Observe(a.Key)
                                                .ObserveOn(invoker.Context)
                                                .Publish().RefCount();

                            var exitNormal = shareOutSx
                                  .Where(b => b.Maybe<bool>("SessionEndByTimeout").ValueOrDefault)
                                 .Throttle(TimeSpan.FromMinutes(2));

                            var exitSessionAbandon = shareOutSx
                                  .Where(b => !b.Maybe<bool>("SessionEndByTimeout").ValueOrDefault)
                                   .Throttle(TimeSpan.FromSeconds(30));

                            var exitSx = Observable.Amb(exitNormal, exitSessionAbandon).Take(1);

                            return exitSx;
                        }).Switch();
                });

            var sx = groupSx.Select(grp => // Console.WriteLine("Compose a Group (Creation) (React just in a first time)");
                        AccumulateLogin(invoker, grp, mainSx));

            var dx = sx.Merge()
                    .Where(a => a.Count > 0);

            var cx = ComposicaoCurriculoParaGatilho(invoker, dx, a => a.Last(), false)
                    .Select(a => new
                    {
                        EventData = a,
                        Task = TaskEx.Run(() => AllinCicloVida.CarregarPorGatilho(BLL.Enumeradores.TipoGatilho.PesquisaVagaCandidato).Memoize())
                    })
                    .DoWithFallBack(async a => await a.Task, (a, ex) => FallBackPlan(invoker, a.EventData.Last(), ex, false))
                    .WhereFallBack(a => a.Task.Result != null && a.Task.Result.Any(), a => ConditionBlockPlan(invoker, a.EventData.Last(), "Ciclo de Vida Inexistente", false))
                    .SelectMany(a => a.Task.Result.ToObservable(invoker.Context), (a, b) => new { a.EventData, LifeCycleTriggerModel = b })
                    .Do(a => a.EventData.Last()["BLL_LifeCycleTriggerModel"] = a.LifeCycleTriggerModel)
                    .Select(a =>
                    new
                    {
                        EventData = a.EventData,
                        Task = PrepararEnvioDeEmailPesquisas(a.EventData)
                    })
                .DoWithFallBack(async a => await a.Task, (next, ex) => FallBackPlan(invoker, next.EventData.Last(), ex, false))
                .WhereFallBack(a => a.Task.Result != null, a => ConditionBlockPlan(invoker, a.EventData.Last(), "Falha ao gerar/encontrar dados de notificação para o serviço externo.", false));

            var serial = new SerialDisposable();
            var sub = cx
                      .Select(a => new
                    {
                        EventData = a.EventData,
                        Task = OutputTriggerToExternalTarget(a.Task.Result)
                    })
                    .DoWithFallBack(async a => await a.Task, (next, ex) => FallBackPlan(invoker, next.EventData.Last(), ex, false))
                    .DoWithFallBack(async a => await AtualizarUltimoEnvioDeEmailBaseadoNasBuscas(a.EventData), (next, ex) => FallBackPlan(invoker, next.EventData.Last(), ex, false))
                    .SubscribeOn(invoker.Context)
                    .Subscribe(next => { }, ex =>
                    {
                        UnhandledException(ex);
                        Scheduler.Schedule(invoker.Context, DateTime.Now.AddSeconds(30), () =>
                        {
                            serial.Disposable = CreateSearchTriggger(invoker);
                        });
                    });
            serial.Disposable = sub;
            return sub;
        }

        private async Task AtualizarUltimoEnvioDeEmailBaseadoNasBuscas(IList<DynObj> list)
        {
            var idCurriculo = list.Last().Maybe<int>("IdCurriculo");

            if (!idCurriculo.Valid)
                return;

            await Task.Factory.StartNew(() =>
           {
               ParametroCurriculo paramCur;
               if (ParametroCurriculo.CarregarParametroPorCurriculo(Parametro.DataEnvioEmailPesquisaMelhoresVagas, new Curriculo(idCurriculo.ValueOrDefault), out paramCur, null))
               {
                   paramCur.ValorParametro = DateTime.Now.ToString(CultureInfo.GetCultureInfo("en-US"));
               }
               else
               {
                   paramCur = new ParametroCurriculo();
                   paramCur.Parametro = new BLL.Parametro((int)Parametro.DataEnvioEmailPesquisaMelhoresVagas);
                   paramCur.Curriculo = new Curriculo(idCurriculo.ValueOrDefault);
                   paramCur.ValorParametro = DateTime.Now.ToString(CultureInfo.GetCultureInfo("en-US"));
               }
               paramCur.Save();
           });

        }

        private async Task OutputTriggerToExternalTarget(NotificaCicloDeVidaAllIn a)
        {
            var output = new EnvioSaidaAllInPainelCicloVida(a);
            await output.Process();
        }

        private async Task<NotificaCicloDeVidaAllIn> PrepararEnvioDeEmailPesquisas(IList<DynObj> list)
        {
            var lastData = list.Last();

            var curId = lastData.Maybe<int>("IdCurriculo");

            if (!curId.Valid)
                return null;

            var cur = lastData.Maybe<AllInCurriculo>("BLL_AllInCurriculo");
            if (!cur.Valid)
                return null;

            var lifeCyle = lastData.Maybe<AllinCicloVida>("BLL_LifeCycleTriggerModel");
            if (!lifeCyle.Valid)
                return null;

            var templateModel = ModelTranslator.TranslateToAllInCurriculoModel(cur.ValueOrDefault);

            var customData = await PegarDadosMelhoresVagas(list,
                                                        cur.ValueOrDefault.IdCurriculo,
                                                        cur.ValueOrDefault.IdPessoaFisica,
                                                        PessoaFisica.PesquisarIdCidadeAcessor(cur.ValueOrDefault.IdPessoaFisica));

            if (customData.Item1 <= 0)
                return null;

            var resultModel = await GenerateResultModel(cur.ValueOrDefault, templateModel, lifeCyle.ValueOrDefault, customData.Item2);

            if (customData.Item1 == 1)
            {
                resultModel.Evento = resultModel.Evento.Replace("{qtd}", "1");
            }
            else if (customData.Item1 == 2)
            {
                resultModel.Evento = resultModel.Evento.Replace("{qtd}", "2");
            }
            else
            {
                resultModel.Evento = resultModel.Evento.Replace("{qtd}", "3");
            }

            return resultModel;
        }

        private async Task<Tuple<int, KeyValuePair<string, string>[]>> PegarDadosMelhoresVagas(IList<DynObj> list, int idCurriculo, int idPessoaFisica, Func<int> idCidadeCurriculo)
        {
            return await Task.Factory.StartNew<Tuple<int, KeyValuePair<string, string>[]>>(() =>
                {
                    var ordered = list.OrderByDescending(a => a.Maybe<DateTime>("Execution").ValueOrDefault);

                    var grouped = ordered.GroupBy(a => a.Maybe<int>("IdFuncao").ValueOrDefault)
                                         .ToDictionary(a => a.Key, b => b.ToArray())
                                         .OrderByDescending(a => a.Value.Length)
                                         .Take(3)
                                         .ToArray();

                    var buffer = new Dictionary<int, TipoGatilho.MelhorVagaModel[]>();
                    foreach (var item in grouped)
                    {
                        var utilizandoCidade = item.Value.FirstOrDefault(a => a.Maybe<int>("IdCidade").ValueOrDefault > 0);

                        IEnumerable<TipoGatilho.MelhorVagaModel> res;
                        if (utilizandoCidade != null)
                            res = TipoGatilho.RetornarMelhoresVagas(item.Key, utilizandoCidade.Maybe<int>("IdCidade").ValueOrDefault, idCurriculo);
                        else
                        {
                            var cidadeId = idCidadeCurriculo();
                            if (cidadeId > 0)
                                res = TipoGatilho.RetornarMelhoresVagas(item.Key, idCidadeCurriculo(), idCurriculo);
                            else
                                res = Enumerable.Empty<TipoGatilho.MelhorVagaModel>();
                        }

                        buffer.Add(item.Key, res.ToArray());
                    }

                    var vagas = buffer.ToArray().FlattenAsBreadth(a => a.Value).Take(3).ToArray();

                    return new Tuple<int, KeyValuePair<string, string>[]>(vagas.Length, GerarMetadataAllin(vagas));
                });
        }

        private KeyValuePair<string, string>[] GerarMetadataAllin(TipoGatilho.MelhorVagaModel[] vagas)
        {
            var data = new Dictionary<string, string>();
            for (int i = 0; i < vagas.Length; i++)
            {
                var item = vagas[i];
                var index = i + 1;
                data.Add("Profissao_0" + index, item.DescricaoFuncao);
                data.Add("Profissao_0" + index + "_Salario", item.SalarioPara > 0 ? item.SalarioPara.ToString("C2", CultureInfo.GetCultureInfo("pt-BR")) : item.SalarioDe.ToString("C2", CultureInfo.GetCultureInfo("pt-BR")));
                data.Add("Profissao_0" + index + "_Cidade", item.NomeCidade);
                data.Add("Profissao_0" + index + "_Estado", item.Estado);

                var atributos = item.Atributos.Length <= 200 ? item.Atributos : new string((item.Atributos ?? string.Empty).Take(197).Concat(new[] { '.', '.', '.' }).ToArray());
                data.Add("Profissao_0" + index + "_Atributos", atributos);

                var urlVaga = Vaga.MontarUrlVaga(item.IdVaga);
                data.Add("Profissao_0" + index + "_UrlVaga", urlVaga);
            }
            return data.ToArray();
        }

        private async Task<bool> PermitidoEnviarEmailBaseadoEmPesquisasRealizadas(ShootResultArgs<DynObj> a)
        {
            var curId = a.OriginalEvent.Maybe<int>("IdCurriculo");

            if (!curId.Valid)
                return false;

            return await TaskEx.Run(() =>
                {
                    var textLimit = BLL.Parametro.RecuperaValorParametro(Parametro.LimiteDiasEnvioEmailPesquisaMelhoresVagas);

                    int valueLimit;
                    if (string.IsNullOrWhiteSpace(textLimit) || !Int32.TryParse(textLimit, out valueLimit) || valueLimit <= 0)
                        return true;

                    ParametroCurriculo paramCur;
                    if (!ParametroCurriculo.CarregarParametroPorCurriculo(Parametro.DataEnvioEmailPesquisaMelhoresVagas, new Curriculo(curId.ValueOrDefault), out paramCur, null))
                        return true;

                    if (string.IsNullOrWhiteSpace(paramCur.ValorParametro))
                        return true;

                    DateTime lastSend;
                    if (!DateTime.TryParse(paramCur.ValorParametro, CultureInfo.GetCultureInfo("en-US"), DateTimeStyles.None, out lastSend))
                        return true;

                    var res = DateTime.Now - lastSend;

                    if (res.TotalDays > valueLimit)
                        return true;

                    return false;
                });
        }

        private static IObservable<IList<DynObj>> AccumulateLogin(IReactiveFlowInvoker invoker, IGroupedObservable<int, ShootResultArgs<DynObj>> loginGroup, IReactRoutedEvent<DynObj, int> mainSx)
        {
            var closureKey = loginGroup.Key;
            return loginGroup.Publish(share =>   // Console.WriteLine("Publish a Group (Next) to share your use (React just in a first time)");
                    AccumulateSearch(invoker, closureKey, share, mainSx));
        }

        private static IObservable<IList<DynObj>> AccumulateSearch(IReactiveFlowInvoker invoker, int loginGroupKey, IObservable<ShootResultArgs<DynObj>> share, IReactRoutedEvent<DynObj, int> mainSx)
        {
            return share.Take(1)
                    .Select(c =>
                               {
                                   //	Console.WriteLine("Select Searchs");
                                   return mainSx.Observe(loginGroupKey).Do(a => a["Execution"] = DateTime.Now).TakeUntil(share.LastOrDefaultAsync())
                                        .Buffer(() => Observable.Amb(Observable.Do(share.LastOrDefaultAsync(), d => c.OriginalEvent["FinalizedOk"] = true)
                                                                                            .Select(obj => Unit.Default),
                                                                        Observable.Do(Observable.Timer(TimeSpan.FromHours(8)).ObserveOn(invoker.Context), d => c.OriginalEvent["FinalizedTimeout"] = true)
                                                                                            .Select(obj => Unit.Default)
                                                                    )
                                                );
                               }
                    ).Merge();
        }

        private IDisposable CreateIgnorableSubs(IReactiveFlowInvoker invoker, string details, params IObservable<DynObj>[] subs)
        {
            if (subs == null)
                return Disposable.Empty;

            var serial = new SerialDisposable();
            serial.Disposable = subs.Merge(invoker.Context)
                 .Do(a =>
                     {
                         a["ToIgnore"] = true;
                         //a["Details"] = "Gatilho Não Implementado";
                         a["Details"] = details;
                         invoker.Mediator.PublishIfExists<DynObj>(EventTitleToAllin.GeneralOutput, a);
                     })
                .SubscribeOn(invoker.Context)
                .Subscribe(next => { }, ex =>
                {
                    UnhandledException(ex);
                    Scheduler.Schedule(invoker.Context, DateTime.Now.AddSeconds(30), () =>
                        {
                            serial.Disposable = CreateIgnorableSubs(invoker, details, subs);
                        });
                });
            return serial;
        }

        private IDisposable CreateMainSubs(IReactiveFlowInvoker invoker)
        {
            var serial = new SerialDisposable();
            serial.Disposable = invoker.Mediator
                    .CreateIfNotExists<DynObj, int>(EventTitleToAllin.GeneralInput, a => a.Maybe<int>("Key").ValueOrDefault)
                    .Broadcast()
                    .ObserveOn(invoker.Context)
                    .Select(a =>
                        new
                        {
                            EventData = a,
                            Task = TaskEx.Run(() =>
                                TipoGatilho.GatilhoAtivo(a.OriginalEvent.Maybe<int>("Key").ValueOrDefault))
                        })
                .DoWithFallBack(async a => await a.Task, (next, ex) => FallBackPlan(invoker, next.EventData.OriginalEvent, ex))
                .WhereFallBack(a => a.Task.Result, a => ConditionBlockPlan(invoker, a.EventData.OriginalEvent, "Gatilho Inativo"))
                .Do(next => RouteEvent(invoker, next.EventData))
                .SubscribeOn(invoker.Context)
                .Subscribe(next => { }, ex =>
                    {
                        UnhandledException(ex);
                        Scheduler.Schedule(invoker.Context, DateTime.Now.AddSeconds(30), () =>
                            {
                                serial.Disposable = CreateMainSubs(invoker);
                            });
                    });

            return serial;
        }

        private void UnhandledException(Exception ex)
        {
            GerenciadorException.GravarExcecao(ex);
        }

        private static void RouteEvent(IReactiveFlowInvoker invoker, ShootResultArgs<DynObj> data)
        {
            var handler = MappingInvokerEvent().FirstOrDefault(a => a.Key == (BLL.Enumeradores.TipoGatilho)data.OriginalEvent.Maybe<int>("Key").ValueOrDefault);

            if (handler.Value == null)
                throw new NotImplementedException("TipoGatilho undefined");

            handler.Value(invoker, data.OriginalEvent);
        }

        private void ConditionBlockPlan(IReactiveFlowInvoker invoker, DynObj arg, string details, bool routeToOutput = true)
        {
            if (!routeToOutput)
                return;

            arg["ToIgnore"] = true;
            arg["Details"] = details;
            invoker.Mediator.PublishIfExists<DynObj>(EventTitleToAllin.GeneralOutput, arg);
            return;
        }

        private void FallBackPlan(IReactiveFlowInvoker invoker, DynObj arg, Exception exception, bool routeToOutput = true)
        {
            if (routeToOutput)
            {
                arg["Error"] = true;
                arg["Exception"] = new TriggerNowException(exception);
                invoker.Mediator.PublishIfExists<DynObj>(EventTitleToAllin.GeneralOutput, arg);
                return;
            }
            GerenciadorException.GravarExcecao(exception);
        }

        private IObservable<T> ComposicaoCurriculoParaGatilho<T>(IReactiveFlowInvoker invoker, IObservable<T> observable, Func<T, DynObj> accessor, bool routeToOutput = true)
        {
            return observable.WhereFallBack(a => accessor(a).Maybe<int>("IdCurriculo").ValueOrDefault > 0, a => ConditionBlockPlan(invoker, accessor(a), "IdCurriculo Inválido"))
                          .Select(a => new
                          {
                              EventData = a,
                              Task = TaskEx.Run(() => Curriculo.CarregarCurriculoExportacaoAllIn(accessor(a).Maybe<int>("IdCurriculo").ValueOrDefault, null))
                          })
                         .DoWithFallBack(async a => await a.Task, (a, ex) => FallBackPlan(invoker, accessor(a.EventData), ex, routeToOutput))
                         .WhereFallBack(a => a.Task.Result != null, a => ConditionBlockPlan(invoker, accessor(a.EventData), "Currículo Inválido ou Indisponível", routeToOutput))
                         .Do(a => accessor(a.EventData)["BLL_AllInCurriculo"] = a.Task.Result)
                         .Select(a => a.EventData);
        }

        private IDisposable CreateLifeCycleStandardTrigger(IReactiveFlowInvoker invoker)
        {
            var serial = new SerialDisposable();

            var pre = GetLifeyCycleStandardTriggersTypes(invoker)
                            .Merge(invoker.Context)
                            .Where(a => !a.Maybe<bool>("ToIgnore").ValueOrDefault);

            var disp = ComposicaoCurriculoParaGatilho(invoker, pre, a => a)
                            .Select(a => new
                            {
                                EventData = a,
                                Task = TaskEx.Run(() => AllinCicloVida.CarregarPorGatilho((BLL.Enumeradores.TipoGatilho)a.Maybe<int>("Key").ValueOrDefault).Memoize())
                            })
                            .DoWithFallBack(async a => await a.Task, (a, ex) => FallBackPlan(invoker, a.EventData, ex))
                            .WhereFallBack(a => a.Task.Result != null && a.Task.Result.Any(), a => ConditionBlockPlan(invoker, a.EventData, "Ciclo de Vida Inexistente"))
                            .SelectMany(a => Observable.Using(() => a.Task.Result, r => r.ToObservable(invoker.Context)), (a, b) => new { a.EventData, LifeCycleTriggerModel = b })
                            .Do(a => a.EventData["BLL_LifeCycleTriggerModel"] = a.LifeCycleTriggerModel)
                            .Select(a => new
                            {
                                EventData = a.EventData,
                                Task = GetTemplateModel(a.EventData.Get<AllInCurriculo>("BLL_AllInCurriculo"))
                            })
                            .DoWithFallBack(async a => await a.Task, (a, ex) => FallBackPlan(invoker, a.EventData, ex))
                            .Where(a => a.Task.Result != null)
                            .Do(a => a.EventData["AllInMail_AllInTemplateModel"] = a.Task.Result)
                            .Select(a => new
                            {
                                EventData = a.EventData,
                                Task = GenerateResultModel(a.EventData.Get<AllInCurriculo>("BLL_AllInCurriculo"),
                                                            a.Task.Result,
                                                            a.EventData.Get<AllinCicloVida>("BLL_LifeCycleTriggerModel"))
                            })
                            .DoWithFallBack(async a => await a.Task, (a, ex) => FallBackPlan(invoker, a.EventData, ex))
                            .WhereFallBack(a => a.Task.Result != null, a => ConditionBlockPlan(invoker, a.EventData, "Modelo de Dados para AllIn é Inválido ou Inexistente"))
                            .Do(a => a.EventData["Result"] = a.Task.Result)
                            .Do(a =>
                                {
                                    invoker.Mediator.PublishIfExists<DynObj>(EventTitleToAllin.GeneralOutput, a.EventData);
                                })
                            .SubscribeOn(invoker.Context)
                            .Subscribe(next => { }, ex =>
                            {
                                UnhandledException(ex);
                                Scheduler.Schedule(invoker.Context, DateTime.Now.AddSeconds(30), () =>
                                {
                                    serial.Disposable = CreateLifeCycleStandardTrigger(invoker);
                                });
                            });

            serial.Disposable = disp;
            return serial;

        }

        public async Task<AllInCurriculoTemplateModel> GetTemplateModel(AllInCurriculo bllModel)
        {
            return ModelTranslator.TranslateToAllInCurriculoModel(bllModel);
        }

        private Task<NotificaCicloDeVidaAllIn> GenerateResultModel(AllInCurriculo bllModel,
                                                                        AllInCurriculoTemplateModel allInTemplateModel,
                                                                        AllinCicloVida lifeCycleModel,
                                                                            KeyValuePair<string, string>[] customData = null)
        {
            return Task.Factory.StartNew(() =>
                {
                    var allInConv = new AllInCurriculoDefConverter();
                    var resValue = allInConv.Parse(allInTemplateModel);
                    var defProperties = allInConv.GetDefiniedFields();

                    var defValues = resValue.Split(';');

                    // improve below
                    defValues = defValues.Select(a => a.Length > 100 ?
                                                new string(a.Take(98).Concat(new[] { '.', '.' }).ToArray()) : a)
                                            .ToArray();

                    var pairs = defProperties.Select((a, index) => new KeyValuePair<string, string>(a, defValues[index]));

                    var hash = GetLoginHash(bllModel);

                    var dominio = GetTargetDomainToLogin();
                    var urlLogin = Rota.RecuperarURLRota(RouteCollection.LogarAutomatico);

                    var otherPairs = new[] { 
                                                new KeyValuePair<string, string>("hash", hash),
                                                new KeyValuePair<string, string>("dominio", dominio.EndsWith("/") ? dominio : dominio + "/"),
                                                new KeyValuePair<string, string>("path", urlLogin.Replace("{HashAcesso}", string.Empty)),
                                                new KeyValuePair<string, string>("utm", lifeCycleModel.DescricaoGoogleUtm)
                                            };

                    var res = new NotificaCicloDeVidaAllIn();
                    res.AceitaRepeticao = lifeCycleModel.FlagAceitaRepeticao;
                    res.EmailEnvio = bllModel.Email;
                    res.Evento = lifeCycleModel.DescricaoEvento;
                    res.IdentificadorAllIn = lifeCycleModel.IdentificadorCicloAllin;
                    if (customData == null)
                        res.CamposComValores = pairs.Concat(otherPairs).ToArray();
                    else
                        res.CamposComValores = pairs.Concat(otherPairs).Concat(customData).ToArray();

                    //res.ListaParaAtualizacao;
                    return res;
                });
        }

        private string GetTargetDomainToLogin()
        {
            var url = BLL.Parametro.RecuperaValorParametro(Parametro.URLAmbiente);

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

        public IEnumerable<IObservable<DynObj>> GetLifeyCycleStandardTriggersTypes(IReactiveFlowInvoker invoker)
        {
            yield return invoker.Mediator.GetConsumer<DynObj>(EventTitleToAllin.TriggerRegister);
            yield return invoker.Mediator.GetConsumer<DynObj>(EventTitleToAllin.TriggerStartVIP);
            yield return invoker.Mediator.GetConsumer<DynObj>(EventTitleToAllin.TriggerEndVIP);

            //yield return invoker.Mediator.GetConsumer<DynObj>(EventTitleToAllin.TriggerLogin);
            //yield return invoker.Mediator.GetConsumer<DynObj>(EventTitleToAllin.TriggerCandidateResearchedJob);
            //yield return invoker.Mediator.GetConsumer<DynObj>(EventTitleToAllin.TriggerLogout);
        }

        private static IEnumerable<KeyValuePair<BLL.Enumeradores.TipoGatilho, Action<IReactiveFlowInvoker, DynObj>>> MappingInvokerEvent()
        {
            yield return new KeyValuePair<BLL.Enumeradores.TipoGatilho, Action<IReactiveFlowInvoker, DynObj>>
                (BLL.Enumeradores.TipoGatilho.AcabouVip, (invoker, data) => invoker.Mediator.PublishIfExists<DynObj>(EventTitleToAllin.TriggerEndVIP, data));

            yield return new KeyValuePair<BLL.Enumeradores.TipoGatilho, Action<IReactiveFlowInvoker, DynObj>>
              (BLL.Enumeradores.TipoGatilho.CadastroCurriculo, (invoker, data) => invoker.Mediator.PublishIfExists<DynObj>(EventTitleToAllin.TriggerRegister, data));

            yield return new KeyValuePair<BLL.Enumeradores.TipoGatilho, Action<IReactiveFlowInvoker, DynObj>>
              (BLL.Enumeradores.TipoGatilho.CompraAprovadaVip, (invoker, data) => invoker.Mediator.PublishIfExists<DynObj>(EventTitleToAllin.TriggerStartVIP, data));

            yield return new KeyValuePair<BLL.Enumeradores.TipoGatilho, Action<IReactiveFlowInvoker, DynObj>>
              (BLL.Enumeradores.TipoGatilho.LoginCandidato, (invoker, data) => invoker.Mediator.PublishIfExists<DynObj>(EventTitleToAllin.TriggerLogin, data));

            yield return new KeyValuePair<BLL.Enumeradores.TipoGatilho, Action<IReactiveFlowInvoker, DynObj>>
              (BLL.Enumeradores.TipoGatilho.LogoutCandidato, (invoker, data) => invoker.Mediator.PublishIfExists<DynObj>(EventTitleToAllin.TriggerLogout, data));

            yield return new KeyValuePair<BLL.Enumeradores.TipoGatilho, Action<IReactiveFlowInvoker, DynObj>>
             (BLL.Enumeradores.TipoGatilho.PesquisaVagaCandidato, (invoker, data) => invoker.Mediator.PublishIfExists<DynObj>(EventTitleToAllin.TriggerCandidateResearchedJob, data));

        }

    }

    public static class TriggerProcessorEx
    {
        public static IEnumerable<V> FlattenAsBreadth<T, V>(this IList<T> col, Func<T, IList<V>> accessor)
        {
            int count = 0;
            int max = 0;
            var dict = new Dictionary<T, int>();

            do
            {
                foreach (var item in col)
                {
                    dict[item] = count;

                    var nested = accessor(item);

                    max = Math.Max(max, nested.Count);
                    if (nested.Count > count)
                        yield return nested[count];
                }

                count++;

            } while (dict.All(a => a.Value != max));

        }
    }
}
