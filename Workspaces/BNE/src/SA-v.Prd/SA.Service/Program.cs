using System;
using System.Collections.Specialized;
using System.Reflection;
using Autofac;
using Autofac.Extras.Quartz;
using log4net;
using Quartz;
using SA.Infra.CrossCutting.IoC;
using Topshelf;
using Topshelf.Autofac;
using Topshelf.Quartz;

namespace SA.Service
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
                builder.RegisterModule(new QuartzAutofacJobsModule(typeof(OneSignalNotificarCompraNaoFinalizada).Assembly));
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
                            q.WithJob(() => JobBuilder.Create<OneSignalNotificarCompraNaoFinalizada>().WithIdentity("NotificarCompraNaoFinalizada", null).Build());
                            q.AddTrigger(() => TriggerBuilder.Create().WithSchedule(SimpleScheduleBuilder.RepeatMinutelyForever(5)).Build());
                        });

                        s.ScheduleQuartzJob(q =>
                        {
                            q.WithJob(() => JobBuilder.Create<OneSignalNotificarEmpresaNaoEstaUsandoSite>().WithIdentity("NotificarEmpresaNaoEstaUsandoSite", null).Build());
                            q.AddTrigger(() => TriggerBuilder.Create().WithCronSchedule("0 0 10 1/1 * ? *").Build());
                        });


                        _logger.Info("Job configured..");
                    });

                    c.RunAsNetworkService();
                    c.StartAutomatically();

                    c.SetDescription("SA Services");
                    c.SetDisplayName("SA Services");
                    c.SetServiceName("SA.Services");


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