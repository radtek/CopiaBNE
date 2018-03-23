using Autofac;
using BNE.ExceptionLog;
using BNE.ExceptionLog.Interface;

namespace BNE.Services.AsyncServices.Base
{
    public class Autofac
    {
        private IContainer _autofacContainer;

        protected IContainer AutofacContainer
        {
            get
            {
                if (_autofacContainer == null)
                {
                    var builder = new ContainerBuilder();

                    builder.RegisterType<WebAPILogger>().As<ILogger>().InstancePerLifetimeScope();

                    IContainer container = builder.Build();

                    _autofacContainer = container;
                }

                return _autofacContainer;
            }
        }
        
        public ILogger Logger
        {
            get { return AutofacContainer.Resolve<ILogger>(); }
        }
    }
}
