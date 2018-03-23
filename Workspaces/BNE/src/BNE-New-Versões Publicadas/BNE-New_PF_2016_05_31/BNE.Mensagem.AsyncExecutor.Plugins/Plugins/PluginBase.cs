using Autofac;
using BNE.Data.Infrastructure;
using BNE.ExceptionLog;
using BNE.ExceptionLog.Interface;
using BNE.Mensagem.Data.Infrastructure;
using BNE.Mensagem.Data.Repositories;

namespace BNE.Mensagem.AsyncExecutor.Plugins.Plugins
{
    public abstract class PluginBase : Services.AsyncServices.Base.Plugins.PluginBase
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
                    builder.RegisterType<WebAPILogger>().As<ILogger>().InstancePerLifetimeScope();

                    builder.RegisterAssemblyTypes(typeof(Domain.SMS).Assembly).InstancePerLifetimeScope();
                    builder.RegisterAssemblyTypes(typeof(SistemaRepository).Assembly).Where(t => t.Name.EndsWith("Repository")).AsImplementedInterfaces().InstancePerLifetimeScope();

                    IContainer container = builder.Build();

                    _autofacContainer = container;
                }

                return _autofacContainer;
            }
        }

        protected Domain.Email DomainEmail
        {
            get { return AutofacContainer.Resolve<Domain.Email>(); }
        }

    }
}
