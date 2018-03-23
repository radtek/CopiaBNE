using System;
using BNE.BLL;
using Quartz;

namespace BNE.Services.InscritosSTC
{
    [DisallowConcurrentExecution]
    public partial class InscritosSTC : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                Filial.CartaInscritos();
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "Erro robo Inscritos STC");
            }

        }
    }

}
