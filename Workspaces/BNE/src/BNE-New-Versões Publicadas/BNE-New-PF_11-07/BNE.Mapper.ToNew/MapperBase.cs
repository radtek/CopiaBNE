using Autofac;
using BNE.Data.Infrastructure;
using BNE.Global.Data.Repositories;
using BNE.Logger;
using BNE.Logger.Interface;
using BNE.PessoaJuridica.Data.Infrastructure;
using BNE.PessoaJuridica.Data.Repositories;

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
                    builder.RegisterType<DatabaseFactory>().As<IDatabaseFactory>().InstancePerLifetimeScope();
                    builder.RegisterType<DatabaseLogger>().As<ILogger>().InstancePerLifetimeScope();

                    builder.RegisterAssemblyTypes(typeof(BNE.PessoaJuridica.Domain.PessoaJuridica).Assembly).InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);
                    builder.RegisterAssemblyTypes(typeof(PessoaJuridicaRepository).Assembly).Where(t => t.Name.EndsWith("Repository")).AsImplementedInterfaces().InstancePerLifetimeScope();

                    builder.RegisterAssemblyTypes(typeof(Global.Domain.Cidade).Assembly).InstancePerLifetimeScope();
                    builder.RegisterAssemblyTypes(typeof(CidadeRepository).Assembly).Where(t => t.Name.EndsWith("Repository")).AsImplementedInterfaces().InstancePerLifetimeScope();

                    IContainer container = builder.Build();

                    _autofacContainer = container;
                }

                return _autofacContainer;
            }
        }

        protected BNE.PessoaJuridica.Domain.PessoaJuridica DomainPessoaJuridica
        {
            get { return AutofacContainer.Resolve<BNE.PessoaJuridica.Domain.PessoaJuridica>(); }

        }
        protected ILogger Logger
        {
            get { return AutofacContainer.Resolve<ILogger>(); }
        }

    }
}
