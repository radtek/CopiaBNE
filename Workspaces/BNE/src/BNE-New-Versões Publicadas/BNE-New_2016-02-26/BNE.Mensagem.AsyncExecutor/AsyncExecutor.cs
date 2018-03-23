using System;
using System.ComponentModel.Composition.Hosting;
using System.Configuration;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;

namespace BNE.Mensagem.AsyncExecutor
{
    partial class AsyncExecutor : ServiceBase
    {
        
        public AsyncExecutor()
        {
            InitializeComponent();

            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e == null)
                return;

            var ex = e.ExceptionObject as Exception;
            if (ex == null)
                return;

            new Services.AsyncServices.Base.Autofac().Logger.Error(ex);
        }

        void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            if (e == null)
                return;

            e.SetObserved();

            new Services.AsyncServices.Base.Autofac().Logger.Error(e.Exception);
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                Thread.Sleep(10000);
                Controller.GetControlerCapabilities += Controller_GetControlerCapabilities;
                Controller.GetPluginCatalog += Controller_GetPluginCatalog;
                Controller.InitializeController();

                Controller.StartController();
            }
            catch (Exception ex)
            {
                new Services.AsyncServices.Base.Autofac().Logger.Error(ex);
                throw;
            }

        }

        System.ComponentModel.Composition.Primitives.ComposablePartCatalog Controller_GetPluginCatalog()
        {
            return new DirectoryCatalog(ConfigurationManager.AppSettings["PathPlugins"]);
        }

        Services.AsyncServices.Base.CoreCapabilities Controller_GetControlerCapabilities()
        {
            return new Capabilities();
        }

        protected override void OnStop()
        {
        }
    }
}
