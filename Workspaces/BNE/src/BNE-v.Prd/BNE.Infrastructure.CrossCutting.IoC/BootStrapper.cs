using System;
using System.Configuration;
using System.IO;
using Autofac;
using AutofacContrib.SolrNet;
using AutofacContrib.SolrNet.Config;
using BNE.Cache;
using BNE.Infrastructure.Services.SolrService;
using BNE.Infrastructure.Services.SolrService.Contract;
using log4net.Config;
using MailSender;
using SharedKernel.Logger;
using SharedKernel.Logger.Interfaces;

namespace BNE.Infrastructure.CrossCutting.IoC
{
    public static class BootStrapper
    {
        public static void RegisterServices(this ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<MailSenderAPI>().As<IMailSenderAPI>().AsImplementedInterfaces().SingleInstance();
            containerBuilder.RegisterType<RuntimeCachingService>().As<ICachingService>().AsImplementedInterfaces().SingleInstance();

            #region Solr
            var jobsCoreURI = ConfigurationManager.AppSettings["SOLR-CoreUri-Jobs"];
            if (!string.IsNullOrWhiteSpace(jobsCoreURI))
            {
                containerBuilder.RegisterType<SolrService>().As<ISolrService>().AsImplementedInterfaces().SingleInstance();
                var cores = new SolrServers
                {
                    new SolrServerElement
                    {
                        Id = "Jobs",
                        DocumentType = typeof(BNE.Infrastructure.Services.SolrService.Model.Vaga).AssemblyQualifiedName,
                        Url = jobsCoreURI
                    }
                };

                containerBuilder.RegisterModule(new SolrNetModule(cores));
            }

            #endregion

            #region [Logger]

            XmlConfigurator.ConfigureAndWatch(new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log4net.config")));
            containerBuilder.RegisterInstance<ILoggerRepository>(new LogRepository()).SingleInstance(); // Log Repository
            containerBuilder.Register(ctx => ctx.Resolve<ILoggerRepository>().GetLogger("BNE")).SingleInstance(); // Log Default

            #endregion [Logger]
        }
    }
}