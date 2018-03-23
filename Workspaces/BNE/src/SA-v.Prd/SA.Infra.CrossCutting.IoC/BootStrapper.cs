using System;
using System.Configuration;
using System.IO;
using Autofac;
using log4net.Config;
using OneSignal.CSharp.SDK;
using SA.WebPush;
using SA.WebPush.Interfaces;
using SharedKernel.Logger;
using SharedKernel.Logger.Interfaces;

namespace SA.Infra.CrossCutting.IoC
{
    public static class BootStrapper
    {
        public static void RegisterServices(this ContainerBuilder containerBuilder)
        {
            var oneSignalApiKey = ConfigurationManager.AppSettings["OneSignal-ApiKey"];

            containerBuilder.RegisterInstance(new OneSignalClient(oneSignalApiKey)).As<IOneSignalClient>().AsImplementedInterfaces().SingleInstance();
            containerBuilder.RegisterType<OneSignalWebPushService>().As<IWebPushService>().SingleInstance();

            #region [Logger]
            XmlConfigurator.ConfigureAndWatch(new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log4net.config")));
            containerBuilder.RegisterInstance<ILoggerRepository>(new LogRepository()).SingleInstance(); // Log Repository
            containerBuilder.Register(ctx => ctx.Resolve<ILoggerRepository>().GetLogger("SA")).SingleInstance(); // Log Default
            #endregion [Logger]
        }
    }
}