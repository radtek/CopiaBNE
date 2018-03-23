using System;
using System.Reflection;
using Autofac;
using BNE.Infrastructure.CrossCutting.IoC;
using BNE.Services.JornalVagas.MessageBroker;
using BNE.Services.JornalVagas.MessageBroker.Contracts;
using log4net;
using Topshelf;
using Topshelf.Autofac;

namespace BNE.Services.JornalVagas.AtualizarAlerta
{
    internal class Program
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static void Main()
        {
            try
            {
                var builder = new ContainerBuilder();
                builder.RegisterServices();
                builder.RegisterType<Serializer>().As<ISerializer>().SingleInstance();
                builder.RegisterType<BrokerConnection>().As<IBrokerConnection>().SingleInstance();

                builder.RegisterType<ContainerService>().AsSelf();
                var container = builder.Build();

                HostFactory.Run(c =>
                {
                    c.UseLog4Net();

                    c.UseAutofacContainer(container);

                    c.Service<ContainerService>(s =>
                    {
                        s.ConstructUsingAutofacContainer();
                        s.WhenStarted(service => service.Start());
                        s.WhenStopped(service => service.Stop());
                    });

                    c.RunAsNetworkService();
                    c.StartAutomatically();

                    c.SetDescription("Projeto com a parte de atualização dos alertas para os currículos que receberam jornal");
                    c.SetDisplayName("BNE.Services.JornalVagas.AtualizarAlerta");
                    c.SetServiceName("BNE.Services.JornalVagas.AtualizarAlerta");
                });
            }
            catch (Exception ex)
            {
                Log.Error("Service could not start.", ex);
                throw;
            }
        }
    }
}