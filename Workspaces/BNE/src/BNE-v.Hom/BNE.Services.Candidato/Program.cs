using System;
using System.Reflection;
using Autofac;
using Autofac.Extras.Quartz;
using BNE.Infra.CrossCutting.IoC;
using log4net;
using Quartz;
using Topshelf;
using Topshelf.Autofac;
using Topshelf.Quartz;

namespace BNE.Services.Candidato
{
    public class Program
    {
        private static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static void Main(string[] args)
        {
            _logger.Info("Starting service...");

            try
            {
                var builder = new ContainerBuilder();

                builder.RegisterServices();
                builder.RegisterType<ContainerService>().AsSelf();
                builder.RegisterModule(new QuartzAutofacJobsModule(typeof(BaseJob).Assembly));
                builder.RegisterModule(new QuartzAutofacFactoryModule());

                var container = builder.Build();

                ScheduleJobServiceConfiguratorExtensions.SchedulerFactory = () => container.Resolve<IScheduler>();

                HostFactory.Run(c =>
                {
                    c.UseLog4Net();

                    c.UseAutofacContainer(container);

                    c.Service<ContainerService>(s =>
                    {
                        _logger.Info("Configuring jobs...");

                        s.ConstructUsingAutofacContainer();
                        s.WhenStarted((service, control) => service.Start());
                        s.WhenStopped((service, control) => service.Stop());

                        s.ScheduleQuartzJob(q =>
                        {
                            q.WithJob(() => JobBuilder.Create<NotificarAtualizacaoCurriculoParaPesquisaComPoucosResultados>().WithIdentity("NotificarAtualizacaoCurriculoParaPesquisaComPoucosResultados", null).Build());
                            q.AddTrigger(() => TriggerBuilder.Create().WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(6, 0)).Build());
                        });

                        _logger.Info("Jobs configured..");
                    });

                    c.RunAsNetworkService();
                    c.StartAutomatically();

                    c.SetDescription("Projeto com os novos processos para candidato");
                    c.SetDisplayName("BNE.Candidato.Services");
                    c.SetServiceName("BNE.Candidato.Services");
                });
            }
            catch (Exception ex)
            {
                _logger.Fatal("Wasted!", ex);
                throw;
            }
            _logger.Info("Stopping service...");
        }
    }

    public class ContainerService
    {
        public bool Start()
        {
            return true;
        }

        public bool Stop()
        {
            return true;
        }
    }
}