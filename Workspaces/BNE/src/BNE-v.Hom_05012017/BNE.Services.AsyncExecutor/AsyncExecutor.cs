using System;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Configuration;
using System.ServiceProcess;
using BNE.EL;
using BNE.Services.AsyncServices;

namespace BNE.Services.AsyncExecutor
{
    public partial class AsyncExecutor : ServiceBase
    {
        public AsyncExecutor()
        {
            InitializeComponent();

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e == null)
                return;

            var ex = e.ExceptionObject as Exception;
            if (ex == null)
                return;

            GerenciadorException.GravarExcecao(ex);
        }

        protected override void OnStart(string[] args)
        {
            Inicializar();
        }

        public void Inicializar()
        {
            try
            {
                Controller.GetControlerCapabilities += Controller_GetControlerCapabilities;
                Controller.GetPluginCatalog += Controller_GetPluginCatalog;
                Controller.InitializeController();

                Controller.StartController();
            }
            catch (Exception ex)
            {
                GerenciadorException.GravarExcecao(ex);
                throw;
            }
        }

        private ComposablePartCatalog Controller_GetPluginCatalog()
        {
            return new DirectoryCatalog(ConfigurationManager.AppSettings["PathPlugins"]);
        }

        private CoreCapabilities Controller_GetControlerCapabilities()
        {
            return new Capabilities();
        }

        protected override void OnStop()
        {
        }
    }
}