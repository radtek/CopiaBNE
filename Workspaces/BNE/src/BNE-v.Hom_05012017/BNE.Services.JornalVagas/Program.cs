using System.ServiceProcess;

namespace BNE.Services.JornalVagas
{
    internal static class Program
    {

        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        private static void Main(string[] args)
        {
#if DEBUG
            new JornalVagas().Execute(null);
#else
            ServiceBase.Run(new JornalVagas());
#endif
        }
    }
}