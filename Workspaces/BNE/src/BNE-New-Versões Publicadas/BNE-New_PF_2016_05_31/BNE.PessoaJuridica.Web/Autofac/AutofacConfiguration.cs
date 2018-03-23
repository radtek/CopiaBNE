using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using BNE.ExceptionLog;
using BNE.ExceptionLog.Interface;

namespace BNE.PessoaJuridica.Web.Autofac
{
    public static class AutofacConfiguration
    {
        public static void Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            builder.RegisterType<WebAPILogger>().As<ILogger>().SingleInstance();

            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}