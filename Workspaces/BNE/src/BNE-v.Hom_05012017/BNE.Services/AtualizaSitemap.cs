using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using BNE.BLL;
using BNE.BLL.CloudTag;
using BNE.BLL.Custom;
using BNE.EL;
using BNE.Services.Base.EventLog;
using BNE.Services.Properties;
using BNE.Services.Code;
using Quartz;
using Quartz.Impl;

namespace BNE.Services
{
    internal enum TaskType
    {
        Geral = 0,
        Vagas = 1,
        Curriculos = 2,
        Empresas = 3
    }

    internal partial class AtualizaSitemap : BaseService
    {
        #region Construtores
        public AtualizaSitemap()
        {
            InitializeComponent();
        }
        #endregion

        #region Propriedades
        private IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
        private EventLogWriter eventLog;

        #endregion

        #region Eventos

        #region OnStart
        protected override void OnStart(string[] args)
        {
            eventLog = new EventLogWriter(Settings.Default.LogName, GetType().ToString());
            ConfigurarScheduler();
        }
        #endregion

        #region OnStop
        protected override void OnStop()
        {
            scheduler.Shutdown(false);
        }
        #endregion

        #endregion

        #region Metodos

        public void ConfigurarScheduler()
        {
            try
            {
                scheduler.Start();

                IJobDetail vagasInativas = JobBuilder.Create<AtualizaSitemapJob>().Build();

                //get setting
                var set = Settings.Default.AtualizarSitemapHoraExecucao.Split(':');

                var hour = Convert.ToInt32(set[0]);
                var min = Convert.ToInt32(set[1]);

                ITrigger trigger;

#if DEBUG
                trigger = TriggerBuilder.Create().StartNow().Build();
#else
                trigger = TriggerBuilder.Create()
                    .WithDailyTimeIntervalSchedule
                      (s =>
                         s.WithIntervalInHours(24)
                            .OnEveryDay()
                            .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(hour, min))
                      )
                    .Build();
#endif
                scheduler.ScheduleJob(vagasInativas, trigger);
            }
            catch (Exception ex)
            {
                string message;
                string id = EL.GerenciadorException.GravarExcecao(ex, out message);
                message = string.Format("{0} - {1}", id, message);
                eventLog.LogEvent(message, EventLogEntryType.Error, Event.ErroExecucao);
            }
        }
    }

    public class AtualizaSitemapJob : IJob
    {
        private static Task[] siteMapTasks = new Task[4];
        private static EventLogWriter eventLog;

        public void Execute(IJobExecutionContext context)
        {
            Atualizar();
        }

        public void Atualizar()
        {
            eventLog = new EventLogWriter(Settings.Default.LogName, GetType().ToString());

            try
            {
                eventLog.LogEvent(string.Format("Iniciou agora {0}.", DateTime.Now),
                    EventLogEntryType.Information, Event.InicioExecucao);

                #region Geral
                siteMapTasks[(int)TaskType.Geral] = Task.Factory.StartNew(() =>
                {
                    try
                    {
                        SiteMapControl objSMGeral = new SiteMapControl(TipoSitemap.Geral, "sitemap");

                        objSMGeral.AdicionarURL("agradecimentos", changefreq.weekly, "0.7");
                        objSMGeral.AdicionarURL("anunciar-vaga-gratis", changefreq.monthly, "0.9");
                        objSMGeral.AdicionarURL("cadastro-de-curriculo-gratis", changefreq.monthly, "0.9");
                        objSMGeral.AdicionarURL("cadastro-de-empresa-gratis", changefreq.monthly, "0.9");
                        objSMGeral.AdicionarURL("fale-com-presidente", changefreq.never, "0.7");
                        objSMGeral.AdicionarURL("login-candidato", changefreq.never, "0.6");
                        objSMGeral.AdicionarURL("Login-selecionadora", changefreq.never, "0.6");
                        objSMGeral.AdicionarURL("lista-de-curriculos", changefreq.hourly, "0.7");
                        objSMGeral.AdicionarURL("vagas-de-emprego", changefreq.hourly, "0.7");
                        objSMGeral.AdicionarURL("pesquisa-de-vagas", changefreq.monthly, "0.9");
                        objSMGeral.AdicionarURL("vip", changefreq.monthly, "0.7");
                        objSMGeral.AdicionarURL("onde-estamos", changefreq.never, "0.9");

                        objSMGeral.EndSiteMapGeneration();
                    }
                    catch (Exception ex)
                    {
                        string message;
                        var id = GerenciadorException.GravarExcecao(ex, out message, "Erro na geração do site map geral");
                        message = string.Format("{0} - {1}", id, message);
                        eventLog.LogEvent(message, EventLogEntryType.Error, Event.ErroExecucao);
                    }
                });
                #endregion

                #region Vaga
                siteMapTasks[(int)TaskType.Vagas] = Task.Factory.StartNew(() =>
                {
                    try
                    {
                        var objSitemapVaga = new SiteMapControl(TipoSitemap.Vaga, "sitemapVaga");

                        //Funcao e Cidade
                        Parallel.ForEach(Vaga.RecuperarVagasPorFuncaoCidadeSiteMap(), new ParallelOptions { MaxDegreeOfParallelism = 4 }, vaga =>
                        {
                            var url = SitemapHelper.MontarUrlVagasPorFuncaoCidade(vaga.DescricaoFuncao, vaga.NomeCidade, vaga.SiglaEstado);

                            if (!string.IsNullOrWhiteSpace(url))
                                objSitemapVaga.AdicionarURL(url, changefreq.daily, "0.9");
                        });

                        //Palavras Chave
                        Parallel.ForEach(PalavraFuncaoVaga.RecuperarPalavrasSitemap(), new ParallelOptions { MaxDegreeOfParallelism = 4 }, vaga =>
                        {
                            var url = SitemapHelper.MontarUrlVagasPorPalavraChave(vaga.PalavraChave);

                            if (!string.IsNullOrWhiteSpace(url))
                                objSitemapVaga.AdicionarURL(url, changefreq.daily, "0.9");
                        });

                        //Funcoes
                        Parallel.ForEach(Vaga.RecuperarVagasPorFuncaoSiteMap(), new ParallelOptions { MaxDegreeOfParallelism = 4 }, vaga =>
                        {
                            var url = SitemapHelper.MontarUrlVagasPorFuncao(vaga.DescricaoFuncao);

                            if (!string.IsNullOrWhiteSpace(url))
                                objSitemapVaga.AdicionarURL(url, changefreq.daily, "0.8");
                        });

                        //Cidade
                        Parallel.ForEach(Vaga.RecuperarVagasPorCidadeSiteMap(), new ParallelOptions { MaxDegreeOfParallelism = 4 }, vaga =>
                        {
                            var url = SitemapHelper.MontarUrlVagasPorCidade(vaga.NomeCidade, vaga.SiglaEstado);

                            if (!string.IsNullOrWhiteSpace(url))
                                objSitemapVaga.AdicionarURL(url, changefreq.daily, "0.8");
                        });

                        //Vagas
                        var listaVagaSitemap = Vaga.RecuperarVagasSiteMap();
                        Parallel.ForEach(listaVagaSitemap, new ParallelOptions { MaxDegreeOfParallelism = 4 }, vaga =>
                        {
                            var url = SitemapHelper.MontarUrlVaga(
                                vaga.DescricaoFuncao,
                                vaga.DescricaoAreaBNE,
                                vaga.NomeCidade,
                                vaga.SiglaEstado,
                                vaga.IdfVaga.Value
                                );

                            if (!string.IsNullOrWhiteSpace(url))
                                objSitemapVaga.AdicionarURL(url, changefreq.daily, "0.7");
                        });

                        objSitemapVaga.EndSiteMapGeneration();
                    }
                    catch (Exception ex)
                    {
                        string message;
                        var id = GerenciadorException.GravarExcecao(ex, out message, "Erro na geração do site map de Vagas");
                        message = string.Format("{0} - {1}", id, message);
                        eventLog.LogEvent(message, EventLogEntryType.Error, Event.ErroExecucao);
                    }
                });
                #endregion

                #region Currículo
                siteMapTasks[(int)TaskType.Curriculos] = Task.Factory.StartNew(() =>
                {
                    try
                    {
                        var objSitemapCurriculo = new SiteMapControl(TipoSitemap.Curriculo, "sitemapCurriculo");

                        // Cvs por funcao e cidade
                        Parallel.ForEach(Curriculo.RecuperarCurriculosPorFuncaoCidadeSiteMap(), new ParallelOptions { MaxDegreeOfParallelism = 4 },
                            curriculo =>
                            {
                                objSitemapCurriculo.AdicionarURL(SitemapHelper.MontarUrlCurriculosPorFuncaoCidade(curriculo.DescricaoFuncao, curriculo.NomeCidade, curriculo.SiglaEstado), changefreq.daily, "0.9");
                            });

                        // Cvs por funcao
                        Parallel.ForEach(Curriculo.RecuperarCurriculosPorFuncaoSiteMap(), new ParallelOptions { MaxDegreeOfParallelism = 4 },
                            curriculo =>
                            {
                                objSitemapCurriculo.AdicionarURL(SitemapHelper.MontarUrlCurriculosPorFuncao(curriculo.DescricaoFuncao), changefreq.daily, "0.8");
                            });

                        // Cvs por cidade
                        Parallel.ForEach(Curriculo.RecuperarCurriculosPorCidadeSiteMap(), new ParallelOptions { MaxDegreeOfParallelism = 4 },
                            curriculo =>
                            {
                                objSitemapCurriculo.AdicionarURL(SitemapHelper.MontarUrlCurriculosPorCidade(curriculo.NomeCidade, curriculo.SiglaEstado), changefreq.daily, "0.8");
                            });

                        // Visualizacao de CV
                        Parallel.ForEach(Curriculo.RecuperarCurriculosSiteMap(), new ParallelOptions { MaxDegreeOfParallelism = 4 },
                            curriculo =>
                            {
                                objSitemapCurriculo.AdicionarURL(SitemapHelper.MontarUrlCurriculo(
                                                                        curriculo.DescricaoFuncao,
                                                                        curriculo.NomeCidade,
                                                                        curriculo.SiglaEstado,
                                                                        curriculo.IdfCurriculo.Value
                                                                    ),
                                                                 changefreq.monthly, "0.7");
                            });

                        objSitemapCurriculo.EndSiteMapGeneration();
                    }
                    catch (Exception ex)
                    {
                        string message;
                        var id = GerenciadorException.GravarExcecao(ex, out message, "Erro na geração do site map de Currículos");
                        message = string.Format("{0} - {1}", id, message);
                        eventLog.LogEvent(message, EventLogEntryType.Error, Event.ErroExecucao);
                    }
                });
                #endregion

                #region Empresa
                siteMapTasks[(int)TaskType.Empresas] = Task.Factory.StartNew(() =>
                {
                    try
                    {
                        var objSitemapEmpresa = new SiteMapControl(TipoSitemap.Empresa, "sitemapEmpresa");

                        var listaOrigensSitemap = Origem.RecuperarOrigensSiteMap();

                        Parallel.ForEach(listaOrigensSitemap, new ParallelOptions { MaxDegreeOfParallelism = 4 },
                            origemSitemap =>
                            {
                                var url = SitemapHelper.MontarUrlEmpresa(origemSitemap.DesDiretorio);

                                objSitemapEmpresa.AdicionarURL(url, changefreq.daily, "0.9");
                            });

                        objSitemapEmpresa.EndSiteMapGeneration();
                    }
                    catch (Exception ex)
                    {
                        string message;
                        var id = GerenciadorException.GravarExcecao(ex, out message, "Erro na geração do site map de Empresas");
                        message = string.Format("{0} - {1}", id, message);
                        eventLog.LogEvent(message, EventLogEntryType.Error, Event.ErroExecucao);
                    }
                });
                #endregion

                Task.WaitAll(siteMapTasks);

                eventLog.LogEvent(string.Format("Terminou agora {0}.", DateTime.Now),
                    EventLogEntryType.Information, Event.FimExecucao);
            }
            catch (Exception ex)
            {
                string message;
                var id = GerenciadorException.GravarExcecao(ex, out message);
                message = string.Format("{0} - {1}", id, message);
                eventLog.LogEvent(message, EventLogEntryType.Error, Event.ErroExecucao);
            }
        }

        #endregion
    }
}