using System.ServiceProcess;

namespace BNE.Services.NotificacoesVip
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
#if DEBUG
            new NotificacoesVip().Execute(null);
#else
            ServiceBase.Run(new NotificacoesVip());
#endif
        }
    }
}
