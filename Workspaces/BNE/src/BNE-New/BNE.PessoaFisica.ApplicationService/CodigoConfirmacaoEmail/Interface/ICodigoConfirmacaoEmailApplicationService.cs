using BNE.PessoaFisica.ApplicationService.CodigoConfirmacaoEmail.Command;

namespace BNE.PessoaFisica.ApplicationService.CodigoConfirmacaoEmail.Interface
{
    public interface ICodigoConfirmacaoEmailApplicationService
    {
        string GetByCodigo(GetByCodigoCommand command);
        string GerarCodigoConfirmacao(string email);
    }
}