using BNE.BLL.AsyncServices;
using BNE.Services.Base.Threading;
using System;
using Enumeradores = BNE.BLL.AsyncServices.Enumeradores;

namespace BNE.Services.AsyncExecutor
{
    /// <summary>
    /// Thread que limpa os arquivos anexos e gerados
    /// </summary>
    public class CleanFiles:Worker
    {
        /// <inheridoc/>
        protected override void BeginTask(object objParameter)
        {
            String hora = Parametro.RecuperaValorParametro(Enumeradores.Parametro.HoraExecucaoDelecao);

            while (true)
            {
                String horaAtual = DateTime.Now.ToString("HH:mm");

                if (String.Equals(hora,horaAtual))
                {
                    Controller.CleanFiles();
                }
                Sleep(400);                
            }
        }
    }
}