using Autofac;
using Autofac.Integration.WebApi;
using System.Reflection;
using BNE.Data.Infrastructure;
using BNE.Global.Data.Repositories;
using BNE.PessoaFisica.Data.Infrastructure;
using BNE.PessoaFisica.Data.Repositories;
using System.Web.Http;
using BNE.ExceptionLog;
using BNE.ExceptionLog.Interface;

namespace BNE.PessoaFisica.WebAPI.Autofac
{
    public class AutofacConfiguration
    {
        public static void Configure()
        {
            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<UnitOfWorks>().As<IUnitOfWork>().InstancePerApiRequest();
            builder.RegisterType<DatabaseFactory>().As<IDatabaseFactory>().InstancePerApiRequest();
            builder.RegisterType<WebAPILogger>().As<ILogger>().SingleInstance();

            builder.RegisterAssemblyTypes(typeof(Domain.RankingEmail).Assembly).InstancePerRequest().PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);
            builder.RegisterAssemblyTypes(typeof(PreCurriculoRepository).Assembly).Where(t => t.Name.EndsWith("Repository")).AsImplementedInterfaces().InstancePerRequest();

            builder.RegisterAssemblyTypes(typeof(Global.Domain.Cidade).Assembly).InstancePerRequest();
            builder.RegisterAssemblyTypes(typeof(FuncaoSinonimoRepository).Assembly).Where(t => t.Name.EndsWith("Repository")).AsImplementedInterfaces().InstancePerRequest();

            IContainer container = builder.Build();
            var config = GlobalConfiguration.Configuration;
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}