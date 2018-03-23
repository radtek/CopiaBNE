using System;
using System.IO;
using Autofac;
using log4net.Config;
using MailSender;
using SharedKernel.Logger;
using SharedKernel.Logger.Interfaces;

namespace BNE.Infra.CrossCutting.IoC
{
    public static class BootStrapper
    {
        public static void RegisterServices(this ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<MailSenderAPI>().As<IMailSenderAPI>().AsImplementedInterfaces().SingleInstance();

            #region [Logger]

            XmlConfigurator.ConfigureAndWatch(new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log4net.config")));
            containerBuilder.RegisterInstance<ILoggerRepository>(new LogRepository()).SingleInstance(); // Log Repository
            containerBuilder.Register(ctx => ctx.Resolve<ILoggerRepository>().GetLogger("BNE")).SingleInstance(); // Log Default

            #endregion [Logger]
        }
    }
}