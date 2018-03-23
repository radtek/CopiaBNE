using System;
using BNE.EL;
using Quartz;

namespace BNE.Services.CampanhaRecrutamento
{
    [DisallowConcurrentExecution]
    public class CampanhaRecrutamentoJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                var campanhas = BLL.CampanhaRecrutamento.CampanhasEmAberto();

                foreach (var objCampanhaRecrutamento in campanhas)
                {
                    try
                    {
                        objCampanhaRecrutamento.DispararGatilhoParaAssincrono();
                    }
                    catch (Exception ex)
                    {
                        GerenciadorException.GravarExcecao(ex, "Falha ao processar campanha.");
                    }
                }
            }
            catch (Exception ex)
            {
                GerenciadorException.GravarExcecao(ex);
            }
        }
    }
}
