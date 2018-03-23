using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using BNE.Data.Infrastructure;
using BNE.Logger.Interface;
using BNE.Global.Data.Repositories;
using BNE.Logger;
using BNE.PessoaJuridica.Data.Infrastructure;
using BNE.PessoaJuridica.Data.Repositories;

namespace BNE.PessoaJuridica.WebAPI.Autofac
{
    public static class AutofacConfiguration
    {
        public static void Configure()
        {
            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();
            builder.RegisterType<DatabaseFactory>().As<IDatabaseFactory>().InstancePerRequest();

            builder.RegisterType<DatabaseLogger>().As<ILogger>().SingleInstance();
            builder.RegisterAssemblyTypes(typeof(Domain.PessoaJuridica).Assembly).InstancePerRequest().PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);
            builder.RegisterAssemblyTypes(typeof(PessoaJuridicaRepository).Assembly).Where(t => t.Name.EndsWith("Repository")).AsImplementedInterfaces().InstancePerRequest();

            builder.RegisterAssemblyTypes(typeof(Global.Domain.Cidade).Assembly).InstancePerRequest();
            builder.RegisterAssemblyTypes(typeof(FuncaoSinonimoRepository).Assembly).Where(t => t.Name.EndsWith("Repository")).AsImplementedInterfaces().InstancePerRequest();

            IContainer container = builder.Build();

            var config = GlobalConfiguration.Configuration;
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}