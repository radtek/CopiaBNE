using System;
using System.IO;
using Autofac;
using SharedKernel.Repositories.Contracts;
using BNE.Global.Data.Repositories;
using BNE.PessoaJuridica.Data.Repositories;
using CrossCutting.Infrastructure.Transaction;
using log4net;
using log4net.Config;
using SharedKernel.Logger;
using SharedKernel.Logger.Interfaces;

namespace BNE.Mapper.ToNew
{
    public abstract class MapperBase
    {

        private IContainer _autofacContainer;

        protected IContainer AutofacContainer
        {
            get
            {
                if (_autofacContainer == null)
                {
                    var builder = new ContainerBuilder();

                    builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
                    
                    #region [Logger]
                    XmlConfigurator.ConfigureAndWatch(new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log4Net.config")));

                    var lr = new LogRepository();
                    builder.RegisterInstance<ILoggerRepository>(lr).SingleInstance(); // Log Repository
                    builder.RegisterInstance(lr.GetLogger("Mapper")).SingleInstance(); // Log Default
                    #endregion [Logger]

                    builder.RegisterAssemblyTypes(typeof(BNE.PessoaJuridica.Domain.Services.PessoaJuridicaService).Assembly).InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);
                    builder.RegisterAssemblyTypes(typeof(PessoaJuridicaRepository).Assembly).Where(t => t.Name.EndsWith("Repository")).AsImplementedInterfaces().InstancePerLifetimeScope();

                    builder.RegisterAssemblyTypes(typeof(Global.Domain.Cidade).Assembly).InstancePerLifetimeScope();
                    builder.RegisterAssemblyTypes(typeof(CidadeRepository).Assembly).Where(t => t.Name.EndsWith("Repository")).AsImplementedInterfaces().InstancePerLifetimeScope();

                    IContainer container = builder.Build();

                    _autofacContainer = container;
                }

                return _autofacContainer;
            }
        }

        protected BNE.PessoaJuridica.Domain.Services.PessoaJuridicaService DomainPessoaJuridica
        {
            get { return AutofacContainer.Resolve<BNE.PessoaJuridica.Domain.Services.PessoaJuridicaService>(); }

        }
        protected ILog Logger
        {
            get { return AutofacContainer.Resolve<ILog>(); }
        }

    }
}
