using System;
using System.Configuration;
using System.IO;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using AutoMapper;
using log4net.Config;
using SharedKernel.Logger;
using SharedKernel.Logger.Interfaces;

namespace BNE.PessoaFisica.Web.Autofac
{
    public static class AutofacConfiguration
    {
        public static void Register()
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(typeof (MvcApplication).Assembly);
            builder.Register(c => AutoMapper.Register()).As<IMapper>().SingleInstance();

            //Swagger.Codegen
            /*
            builder.RegisterAssemblyTypes(typeof (CodigoConfirmacaoEmailApi).Assembly).Where(p => p.Name.EndsWith("Api"))
                .AsImplementedInterfaces()
                .FindConstructorsWith(t => new[]
                {
                    t.GetConstructor(new[] {typeof (string)})
                })
                .WithParameter("basePath", ConfigurationManager.AppSettings["EnderecoApiPessoaFisica"]).InstancePerRequest();
            */
            //Azure SDK REST Api Client
            builder.RegisterType<API.PessoaFisicaAPI>().As<API.IPessoaFisicaAPI>().WithParameter("baseUri", new Uri(ConfigurationManager.AppSettings["EnderecoApiPessoaFisica"])).InstancePerRequest();

            #region [Logger]
            XmlConfigurator.ConfigureAndWatch(new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log4Net.config")));

            var lr = new LogRepository();
            builder.RegisterInstance<ILoggerRepository>(lr).SingleInstance(); // Log Repository
            builder.RegisterInstance(lr.GetLogger("PessoaFisicaWeb")).SingleInstance(); // Log Default
            #endregion [Logger]

            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}