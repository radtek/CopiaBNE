using System;
using System.Configuration;
using System.Reflection;
using Autofac;
using Autofac.Extras.Quartz;
using BNE.Infrastructure.CrossCutting.IoC;
using BNE.Services.JornalVagas.MessageBroker;
using BNE.Services.JornalVagas.MessageBroker.Contracts;
using log4net;
using Quartz;
using Topshelf;
using Topshelf.Autofac;
using Topshelf.Quartz;

namespace BNE.Services.JornalVagas.Enfileirar
{
    class Program
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly int RunInterval = Convert.ToInt32(ConfigurationManager.AppSettings.Get("RepeatInterval"));
        private static void Main()
        {
            try
            {
                var builder = new ContainerBuilder();
                builder.RegisterServices();
                builder.RegisterType<Serializer>().As<ISerializer>().SingleInstance();
                builder.RegisterType<BrokerConnection>().As<IBrokerConnection>().SingleInstance().AsSelf();
                builder.RegisterType<BLL.Notificacao.JornalVagaService>().SingleInstance();
                builder.RegisterModule(new QuartzAutofacJobsModule(typeof(JornalVagasJob).Assembly));
                builder.RegisterModule(new QuartzAutofacFactoryModule
                {
                    
                });
                var container = builder.Build();

                ScheduleJobServiceConfiguratorExtensions.SchedulerFactory = () => container.Resolve<IScheduler>();

                HostFactory.Run(c =>
                {
                    c.UseLog4Net();

                    c.UseAutofacContainer(container);

                    c.ScheduleQuartzJobAsService(q =>
                    {
                        Logger.Info("Configurando job para o jornal");

                        q.WithJob(() => JobBuilder.Create<JornalVagasJob>().WithIdentity("JornalVagas.Enfileirar", null).Build());
                        q.AddTrigger(() => TriggerBuilder.Create().WithSchedule(SimpleScheduleBuilder.RepeatSecondlyForever(RunInterval)).Build());
                        
                        Logger.Info("Configuração finalizada da job para o jornal");
                    });

                    c.RunAsNetworkService();
                    c.StartAutomatically();

                    c.SetDescription("Projeto com a parte do enfileiramento do jornal");
                    c.SetDisplayName("BNE.Services.JornalVagas.Enfileirar");
                    c.SetServiceName("BNE.Services.JornalVagas.Enfileirar");
                });
            }
            catch (Exception ex)
            {
                Logger.Error("Service could not start.", ex);
                throw;
            }
        }
    }
}
