using BNE.Dashboard.Entities;
using System;

namespace BNE.Dashboard.Business.Helper
{
    public class WindowsService
    {

        public static void VerifyBusinessRule(Entities.Watcher watcher)
        {
            switch ((Enumerators.WindowsServiceName)Enum.Parse(typeof(Enumerators.WindowsServiceName), watcher.WindowsService.WindowsServiceName))
            {
                case Enumerators.WindowsServiceName.VerificarPlanoNaoEncerradoPessoaFisica:
                    watcher.Amount = BLL.PlanoAdquirido.QuantidadePlanosPessoaFisicaEmAberto();
                    if (watcher.Amount > 0)
                        watcher.Status = Status.ERROR;
                    break;
                case Enumerators.WindowsServiceName.VerificarPlanoNaoEncerradoPessoaJuridica:
                    watcher.Amount = BLL.PlanoAdquirido.QuantidadePlanosPessoaJuridicaEmAberto();
                    if (watcher.Amount > 0)
                        watcher.Status = Status.ERROR;
                    break;
                case Enumerators.WindowsServiceName.VerificarQuantidadeCurriculoVIPComPlanoEncerrado:
                    watcher.Amount = BLL.PlanoAdquirido.QuantidadeCurriculoVIPComPlanoEncerrado();
                    if (watcher.Amount > 0)
                        watcher.Status = Status.ERROR;
                    break;
                case Enumerators.WindowsServiceName.VerificarQuantidadeNotaAntecipadaNaoEnviada:
                    watcher.Amount = BLL.PlanoAdquirido.QuantidadeNotaFiscalAntecipadaNaoEnviada();
                    if (watcher.Amount > 0)
                        watcher.Status = Status.ERROR;
                    break;
            }
        }
    }
}
