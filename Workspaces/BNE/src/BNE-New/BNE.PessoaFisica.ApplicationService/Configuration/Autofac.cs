using System;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using Autofac;
using AutofacContrib.SolrNet;
using AutofacContrib.SolrNet.Config;
using AutoMapper;
using BNE.Cache;
using SharedKernel.Repositories.Contracts;
using BNE.Data.Services;
using BNE.Global.Data.Repositories;
using BNE.Global.Domain;
using BNE.PessoaFisica.ApplicationService.PreCurriculo;
using BNE.PessoaFisica.Data;
using BNE.PessoaFisica.Data.Repositories;
using BNE.PessoaFisica.Domain.Services.SOLR;
using BNE.PessoaFisica.SolrService;
using CrossCutting.Infrastructure.Transaction;
using IdentityModel.Client;
using log4net.Config;
using MailSender;
using SharedKernel.ApplicationService;
using SharedKernel.DomainEvents.Assertation;
using SharedKernel.DomainEvents.Core;
using SharedKernel.DomainEvents.CrossDomainEvents;
using SharedKernel.Logger;
using SharedKernel.Logger.Interfaces;
using SharedKernel.Repositories.Contracts;

namespace BNE.PessoaFisica.ApplicationService.Configuration
{
    public static class Autofac
    {
        public static void UseAutofac(this ContainerBuilder builder, bool forWeb = true)
        {
            #region [Logger]

            XmlConfigurator.ConfigureAndWatch(
                new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log4Net.config")));

            var lr = new LogRepository();
            builder.RegisterInstance<ILoggerRepository>(lr).SingleInstance(); // Log Repository
            builder.RegisterInstance(lr.GetLogger("PessoaFisicaAPI")).SingleInstance(); // Log Default

            #endregion [Logger]

            builder.Register(c => AutoMapper.Register()).As<IMapper>().SingleInstance();

            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();
            builder.RegisterType<PessoaFisicaContext>().As<DbContext>().ResolveScope(forWeb);

            builder.RegisterType<RuntimeCachingService>().As<ICachingService>().SingleInstance();

            builder.RegisterAssemblyTypes(typeof(PreCurriculoApplicationService).Assembly)
                .Where(p => p.Name.EndsWith("Service")).AsImplementedInterfaces().InstancePerRequest();

            //Registering Infrastructure Services
            builder.RegisterAssemblyTypes(typeof(IdentityServerService).Assembly).Where(p => p.Name.EndsWith("Service"))
                .AsImplementedInterfaces().ResolveScope(forWeb);

            //SOLR Service
            //Startup.Init<SolrService.Model.Vaga>(ConfigurationManager.AppSettings["SOLR-CoreUriJobs"]);

            builder.RegisterType<SOLRService>().As<ISOLRService>().InstancePerRequest();
            var cores = new SolrServers
            {
                new SolrServerElement
                {
                    Id = "Jobs",
                    DocumentType = typeof(SolrService.Model.Vaga).AssemblyQualifiedName,
                    Url = ConfigurationManager.AppSettings["SOLR-CoreUriJobs"]
                }
            };

            builder.RegisterModule(new SolrNetModule(cores));

            //Registering TokenClient
            builder.RegisterInstance(new TokenClient(
                    ConfigurationManager.AppSettings["IS-TokenEndpointUri"],
                    ConfigurationManager.AppSettings["IS-ClientID"],
                    ConfigurationManager.AppSettings["IS-ClientSecret"])).As<TokenClient>().AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<MailSenderAPI>().As<IMailSenderAPI>().SingleInstance();

            builder.RegisterAssemblyTypes(typeof(Domain.Model.Vaga).Assembly).InstancePerRequest()
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);
            builder.RegisterAssemblyTypes(typeof(PreCurriculoRepository).Assembly)
                .Where(t => t.Name.EndsWith("Repository")).AsImplementedInterfaces().InstancePerRequest();

            builder.RegisterAssemblyTypes(typeof(Cidade).Assembly).InstancePerRequest();
            builder.RegisterAssemblyTypes(typeof(FuncaoSinonimoRepository).Assembly)
                .Where(t => t.Name.EndsWith("Repository")).AsImplementedInterfaces().InstancePerRequest();

            builder.RegisterType<EventBus>().As<IBus>().ResolveScope(forWeb);
            builder.RegisterType<AutofacDomainEvents>().As<IDomainEvents>().ResolveScope(forWeb);
            builder.RegisterType<CrossDomainEventHandler>().As<IHandler<ICrossDomainEvent>>().ResolveScope(forWeb);
            builder.RegisterType<EventPoolHandler<AssertError>>().AsSelf().As<IHandler<AssertError>>().ResolveScope(forWeb);
        }
    }
}