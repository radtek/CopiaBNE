using BNE.PessoaFisica.ApplicationService.Vaga.Command;

namespace BNE.PessoaFisica.ApplicationService.Vaga.Interface
{
    public interface IVagaApplicationService
    {
        Model.VagaResponse CarregarVaga(GetByIdCommand command);
    }
}
