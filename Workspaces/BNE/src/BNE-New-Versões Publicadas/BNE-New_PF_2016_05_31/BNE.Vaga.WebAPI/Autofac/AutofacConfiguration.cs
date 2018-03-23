using Autofac;
using Autofac.Integration.WebApi;
using System.Reflection;
using BNE.Data.Infrastructure;
using System.Web.Http;
using BNE.ExceptionLog;
using BNE.ExceptionLog.Interface;
using BNE.Vaga.Data.Infrastructure;
using BNE.Vaga.Data.Repositories;

namespace BNE.Vaga.WebAPI.Autofac
{
    public class AutofacConfiguration
    {
        public static void Configure()
        {
            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<UnitOfWorks>().As<IUnitOfWork>().InstancePerRequest();
            builder.RegisterType<DatabaseFactory>().As<IDatabaseFactory>().InstancePerRequest();
            builder.RegisterType<WebAPILogger>().As<ILogger>().SingleInstance();

            builder.RegisterAssemblyTypes(typeof(Domain.Beneficio).Assembly).InstancePerRequest().PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);
            builder.RegisterAssemblyTypes(typeof(BeneficioRepository).Assembly).Where(t => t.Name.EndsWith("Repository")).AsImplementedInterfaces().InstancePerRequest();

            //builder.RegisterAssemblyTypes(typeof(Global.Domain.Cidade).Assembly).InstancePerRequest();
            //builder.RegisterAssemblyTypes(typeof(FuncaoSinonimoRepository).Assembly).Where(t => t.Name.EndsWith("Repository")).AsImplementedInterfaces().InstancePerRequest();

            IContainer container = builder.Build();
            var config = GlobalConfiguration.Configuration;
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}