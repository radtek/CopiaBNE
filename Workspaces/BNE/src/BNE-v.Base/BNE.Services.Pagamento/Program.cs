using System.ServiceProcess;

namespace BNE.Services.Pagamento
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new EmissaoBoletoTerminoVip() 
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
