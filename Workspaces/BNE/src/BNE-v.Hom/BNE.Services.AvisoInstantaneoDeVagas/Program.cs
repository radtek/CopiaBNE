using Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace BNE.Services.AvisoInstantaneoDeVagas
{
    static class Program
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            Logger.Info("Starting service...");
#if DEBUG
            new AvisoDeVagas().ConfigureScheduler();
#else
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new AvisoDeVagas()
            };
            ServiceBase.Run(ServicesToRun);
#endif
            Logger.Info("Stopping service...");
        }
    }
}
