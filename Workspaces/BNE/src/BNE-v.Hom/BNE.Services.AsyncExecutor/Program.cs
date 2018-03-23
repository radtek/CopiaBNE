using System;
using System.ServiceProcess;

namespace BNE.Services.AsyncExecutor
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            /*
            new AsyncExecutor().Inicializar();
            new ManualResetEventSlim(false).Wait();
            */
            try
            {
                var servicesToRun = new ServiceBase[] 
                    { 
                        new AsyncExecutor() 
                    };
                ServiceBase.Run(servicesToRun);
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
                throw;
            }
        }
    }
}
