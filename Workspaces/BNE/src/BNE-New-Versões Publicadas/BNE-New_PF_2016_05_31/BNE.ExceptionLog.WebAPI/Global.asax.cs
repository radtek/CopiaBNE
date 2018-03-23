using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using BNE.ExceptionLog.Data.Infrastructure;
using BNE.ExceptionLog.Data.Repositories;
using BNE.ExceptionLog.LogServer.Helper;
using BNE.ExceptionLog.WebAPI.Autofac;

namespace BNE.ExceptionLog.WebAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {

        private IContainer _autofacContainer;

        protected IContainer AutofacContainer
        {
            get
            {
                if (_autofacContainer == null)
                {
                    var builder = new ContainerBuilder();

                    builder.RegisterType<DatabaseFactory>().As<IDatabaseFactory>().InstancePerLifetimeScope();
                    builder.RegisterAssemblyTypes(typeof(Domain.Error).Assembly).InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);
                    builder.RegisterAssemblyTypes(typeof(ErrorRepository).Assembly).Where(t => t.Name.EndsWith("Repository")).AsImplementedInterfaces().InstancePerLifetimeScope();

                    IContainer container = builder.Build();

                    _autofacContainer = container;
                }

                return _autofacContainer;
            }
        }

        protected Domain.Error DomainError
        {
            get { return AutofacContainer.Resolve<Domain.Error>(); }
        }

        protected Domain.Warning DomainWarning
        {
            get { return AutofacContainer.Resolve<Domain.Warning>(); }
        }

        protected Domain.Information DomainInformation
        {
            get { return AutofacContainer.Resolve<Domain.Information>(); }
        }

        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            AutofacConfiguration.Configure();

            LogServer.LogServerService.Configure();

            //Carregando todos os logs para o logserver
            foreach (var data in DomainError.GetAll())
            {
                var exceptionDetails = data.TraceLog.DumpException();
                var ocorrencias = data.Ocorrencias.Select(x => new LogServer.Model.LogInfo.Ocorrencia { Payload = x.Payload, IncidentTime = x.DataCadastro, Usuario = x.Usuario }).ToList();
                var model = new LogServer.Model.LogInfo(data.Guid, data.Aplicacao, exceptionDetails, data.TraceLog.Message, data.CustomMessage, ocorrencias);

                LogServer.LogServerManager.Instance.Log(model);
            }
            foreach (var data in DomainInformation.GetAll())
            {
                var ocorrencias = data.Ocorrencias.Select(x => new LogServer.Model.LogInfo.Ocorrencia { Payload = x.Payload, IncidentTime = x.DataCadastro, Usuario = x.Usuario }).ToList();
                var model = new LogServer.Model.LogInfo(data.Guid, data.Aplicacao, data.Message, ocorrencias, LogServer.Model.LogLevel.Information);

                LogServer.LogServerManager.Instance.Log(model);
            }
            foreach (var data in DomainWarning.GetAll())
            {
                var ocorrencias = data.Ocorrencias.Select(x => new LogServer.Model.LogInfo.Ocorrencia { Payload = x.Payload, IncidentTime = x.DataCadastro, Usuario = x.Usuario }).ToList();
                var model = new LogServer.Model.LogInfo(data.Guid, data.Aplicacao, data.Message, ocorrencias, LogServer.Model.LogLevel.Warning);

                LogServer.LogServerManager.Instance.Log(model);
            }
        }
    }
}
