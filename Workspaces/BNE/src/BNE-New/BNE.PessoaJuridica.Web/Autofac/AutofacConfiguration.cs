using System;
using System.IO;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using log4net.Config;
using SharedKernel.Logger;
using SharedKernel.Logger.Interfaces;

namespace BNE.PessoaJuridica.Web.Autofac
{
    public static class AutofacConfiguration
    {
        public static void Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            #region [Logger]
            XmlConfigurator.ConfigureAndWatch(new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log4Net.config")));

            var lr = new LogRepository();
            builder.RegisterInstance<ILoggerRepository>(lr).SingleInstance(); // Log Repository
            builder.RegisterInstance(lr.GetLogger("PessoaJuridicaWeb")).SingleInstance(); // Log Default
            #endregion [Logger]

            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}