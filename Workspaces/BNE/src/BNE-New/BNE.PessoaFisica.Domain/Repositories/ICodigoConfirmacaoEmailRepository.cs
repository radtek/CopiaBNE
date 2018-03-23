using BNE.PessoaFisica.Domain.Model;
using SharedKernel.Repositories.Contracts;

namespace BNE.PessoaFisica.Domain.Repositories
{
    public interface ICodigoConfirmacaoEmailRepository : IBaseRepository<CodigoConfirmacaoEmail>
    {
        CodigoConfirmacaoEmail GetByCodigo(string codigo);
    }
}