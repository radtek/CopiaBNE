using Autofac;
using BNE.Data.Infrastructure;
using BNE.Logger;
using BNE.Logger.Interface;
using BNE.Mensagem.Data.Infrastructure;
using BNE.Mensagem.Data.Repositories;

namespace BNE.Mensagem.Domain.Test
{
    public class BaseTest
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
                    builder.RegisterType<DatabaseFactory>().As<IDatabaseFactory>().InstancePerLifetimeScope();
                    builder.RegisterType<ConsoleLogger>().As<ILogger>().InstancePerLifetimeScope();

                    builder.RegisterAssemblyTypes(typeof(SMS).Assembly).InstancePerLifetimeScope();
                    builder.RegisterAssemblyTypes(typeof(SistemaRepository).Assembly).Where(t => t.Name.EndsWith("Repository")).AsImplementedInterfaces().InstancePerLifetimeScope();

                    IContainer container = builder.Build();

                    _autofacContainer = container;
                }

                return _autofacContainer;
            }
        }

        protected Email DomainEmail
        {
            get { return AutofacContainer.Resolve<Email>(); }
        }

    }
}
