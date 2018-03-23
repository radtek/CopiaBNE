using System.Configuration;
using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using BNE.Data.Infrastructure;
using BNE.ExceptionLog;
using BNE.ExceptionLog.Interface;
using BNE.Mensagem.Data.Infrastructure;
using BNE.Mensagem.Data.Repositories;

namespace BNE.Mensagem.WebAPI.Autofac
{
    public static class AutofacConfiguration
    {
        public static void Configure()
        {
            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();
            builder.RegisterType<DatabaseFactory>().As<IDatabaseFactory>().InstancePerRequest();
            builder.RegisterType<WebAPILogger>().As<ILogger>().SingleInstance();

            builder.RegisterAssemblyTypes(typeof(SMSRepository).Assembly).Where(t => t.Name.EndsWith("Repository")).AsImplementedInterfaces().InstancePerRequest();

            builder.RegisterAssemblyTypes(typeof(Domain.SMS).Assembly).InstancePerRequest();

            IContainer container = builder.Build();

            var config = GlobalConfiguration.Configuration;
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}