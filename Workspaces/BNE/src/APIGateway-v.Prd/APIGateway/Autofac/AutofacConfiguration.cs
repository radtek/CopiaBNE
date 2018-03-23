using Autofac;
using Autofac.Integration.WebApi;
using BNE.Data.Infrastructure;
using BNE.ExceptionLog;
using BNE.ExceptionLog.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;

namespace APIGateway.Autofac
{
    public class AutofacConfiguration
    {
        public static void Configure()
        {
            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();
            builder.RegisterType<DatabaseFactory>().As<IDatabaseFactory>().InstancePerRequest();
            builder.RegisterType<APIGateway.Data.APIGatewayContext>().As<DbContext>().SingleInstance();
            builder.RegisterType<DatabaseLogger>().As<ILogger>().SingleInstance();

            builder.RegisterAssemblyTypes(typeof(Domain.Api).Assembly).InstancePerRequest();
            builder.RegisterAssemblyTypes(typeof(ApiRepository).Assembly).Where(t => t.Name.EndsWith("Repository")).AsImplementedInterfaces().InstancePerRequest();

            IContainer container = builder.Build();

            var config = GlobalConfiguration.Configuration;
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}