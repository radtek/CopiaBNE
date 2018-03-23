using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using BNE.Web.Services.Solr.Database.Infrastructure;
using BNE.Web.Services.Solr.Database.Repositories;
using BNE.Web.Services.Solr.Domain;

namespace BNE.Web.Services.Solr
{
    public static class Bootstrapper
    {
        public static void Run()
        {
            SetAutofacContainer();
        }

        private static void SetAutofacContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<DatabaseFactoryCidade>().As<IDatabaseFactory>().AsSelf();
            builder.RegisterType<DatabaseFactoryFuncao>().As<IDatabaseFactory>().AsSelf();
            
            builder.Register(c=> new CidadeNaoEncontradaRepository(new DatabaseFactoryCidade())).AsImplementedInterfaces().InstancePerRequest();
            builder.Register(c=> new FuncaoNaoEncontradaRepository(new DatabaseFactoryFuncao())).AsImplementedInterfaces().InstancePerRequest();

            builder.RegisterType(typeof(CidadeNaoEncontrada)).InstancePerRequest();
            builder.RegisterType(typeof(FuncaoNaoEncontrada)).InstancePerRequest();

            IContainer container = builder.Build();

            var config = GlobalConfiguration.Configuration;
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

    }
}