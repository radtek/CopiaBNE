using System;
using System.Threading;
using System.Threading.Tasks;
using BNE.BLL.AsyncServices;

namespace BNE.Services.AsyncExecutor
{
    /// <summary>
    ///     Thread que limpa os arquivos anexos e gerados
    /// </summary>
    public class CleanFiles
    {
        private void BeginTask()
        {
            var hora = Parametro.RecuperaValorParametro(BLL.AsyncServices.Enumeradores.Parametro.HoraExecucaoDelecao);

            while (true)
            {
                var horaAtual = DateTime.Now.ToString("HH:mm");

                if (string.Equals(hora, horaAtual))
                {
                    Controller.CleanFiles();
                }
                Thread.Sleep(TimeSpan.FromMinutes(1));
            }
        }

        public void Start()
        {
            Task.Run(() => BeginTask());
        }
    }
}