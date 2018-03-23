using Autofac;
using Autofac.Integration.Mvc;
using BNE.Logger.Interface;
using System.Web.Mvc;
using BNE.Logger;

namespace BNE.PessoaFisica.Web.Autofac
{
    public static class AutofacConfiguration
    {
        public static void Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            builder.RegisterType<DatabaseLogger>().As<ILogger>().SingleInstance();

            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}