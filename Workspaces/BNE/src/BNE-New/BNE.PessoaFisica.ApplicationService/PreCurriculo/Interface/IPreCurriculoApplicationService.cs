using BNE.PessoaFisica.ApplicationService.PreCurriculo.Command;
using BNE.PessoaFisica.ApplicationService.PreCurriculo.Model;

namespace BNE.PessoaFisica.ApplicationService.PreCurriculo.Interface
{
    public interface IPreCurriculoApplicationService
    {
        PreCurriculoResponse Cadastrar(SalvarPreCurriculoCommand objPreCurriculo);
    }
}