using System;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using Autofac;
using AutoMapper;
using BNE.Cache;
using SharedKernel.Repositories.Contracts;
using BNE.Data.Services;
using BNE.Global.Data.Repositories;
using BNE.Global.Domain;
using BNE.PessoaJuridica.ApplicationService.PessoaJuridica;
using BNE.PessoaJuridica.Data;
using BNE.PessoaJuridica.Data.Repositories;
using BNE.PessoaJuridica.Domain.Services;
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

namespace BNE.PessoaJuridica.ApplicationService.Config
{
    public static class AutofacConfiguration
    {
        public static ContainerBuilder UseApplicationService(this ContainerBuilder builder, bool forWeb = true)
        {
            #region [Logger]

            XmlConfigurator.ConfigureAndWatch(
                new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log4Net.config")));

            var lr = new LogRepository();
            builder.RegisterInstance<ILoggerRepository>(lr).SingleInstance(); // Log Repository
            builder.RegisterInstance(lr.GetLogger("PessoaJuridicaAPI")).SingleInstance(); // Log Default

            #endregion [Logger]

            builder.Register(c => AutoMapperConfiguration.GetInstance().CreateMapper()).As<IMapper>().SingleInstance();

            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();
            builder.RegisterType<PessoaJuridicaContext>().As<DbContext>().ResolveScope(forWeb);
            builder.RegisterType<RuntimeCachingService>().As<ICachingService>().SingleInstance();

            //Registering Infrastructure Services
            builder.RegisterAssemblyTypes(typeof(IdentityServerService).Assembly).Where(p => p.Name.EndsWith("Service"))
                .AsImplementedInterfaces().ResolveScope(forWeb);

            //Registering TokenClient
            builder.RegisterInstance(new TokenClient(
                    ConfigurationManager.AppSettings["IS-TokenEndpointUri"],
                    ConfigurationManager.AppSettings["IS-ClientID"],
                    ConfigurationManager.AppSettings["IS-ClientSecret"])).As<TokenClient>().AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<MailSenderAPI>().As<IMailSenderAPI>().SingleInstance();

            builder.RegisterAssemblyTypes(typeof(PessoaJuridicaService).Assembly).InstancePerRequest()
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);
            builder.RegisterAssemblyTypes(typeof(PessoaJuridicaRepository).Assembly)
                .Where(t => t.Name.EndsWith("Repository")).AsImplementedInterfaces().InstancePerRequest();

            builder.RegisterAssemblyTypes(typeof(Cidade).Assembly).InstancePerRequest();
            builder.RegisterAssemblyTypes(typeof(FuncaoSinonimoRepository).Assembly)
                .Where(t => t.Name.EndsWith("Repository")).AsImplementedInterfaces().InstancePerRequest();
            builder.RegisterAssemblyTypes(typeof(PessoaJuridicaApplicationService).Assembly).Where(p => p.Name.EndsWith("Service"))
                .AsImplementedInterfaces().InstancePerRequest();

            builder.RegisterType<EventBus>().As<IBus>().ResolveScope(forWeb);
            builder.RegisterType<AutofacDomainEvents>().As<IDomainEvents>().ResolveScope(forWeb);
            builder.RegisterType<CrossDomainEventHandler>().As<IHandler<ICrossDomainEvent>>().ResolveScope(forWeb);
            builder.RegisterType<EventPoolHandler<AssertError>>().AsSelf().As<IHandler<AssertError>>().ResolveScope(forWeb);

            return builder;
        }
    }
}