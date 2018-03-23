
namespace BNE.PessoaFisica.ApplicationService.DadosEmpresa.Interface
{
    public interface IDadosEmpresaApplicationService
    {
        Model.DadosEmpresaResponse getDados(Command.DadosEmpresaCommand command);
    }
}
