using System;
using System.Reflection;
using Autofac;
using log4net;
using SA.Infra.CrossCutting.IoC;
using Topshelf;
using Topshelf.Autofac;

namespace SA.DomainEvents.Service
{
    internal class Program
    {
        private static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static void Main(string[] args)
        {
            _logger.Info("Starting service...");

            try
            {
                var builder = new ContainerBuilder();

                builder.RegisterServices();
                builder.RegisterType<EventsListenerService>();

                var container = builder.Build();

                HostFactory.Run(c =>
                {
                    c.UseLog4Net();

                    c.UseAutofacContainer(container);

                    c.Service<EventsListenerService>(s =>
                    {
                        s.ConstructUsingAutofacContainer();
                        s.WhenStarted(service => service.Start());
                        s.WhenStopped(service => service.Stop());
                    });

                    c.RunAsNetworkService();
                    c.StartAutomatically();

                    c.SetDescription("SA DomainEvents Services");
                    c.SetDisplayName("SA DomainEvents Services");
                    c.SetServiceName("SA.DomainEvents.Services");
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
}