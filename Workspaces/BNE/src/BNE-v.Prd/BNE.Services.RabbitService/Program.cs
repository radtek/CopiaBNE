using System.ServiceProcess;

namespace BNE.Services.RabbitService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            //var teste = new IndexacaoCandidatura();
            //teste.Iniciar();

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new IndexacaoCandidatura()
            };
            ServiceBase.Run(ServicesToRun);

           
        }
    }
}
