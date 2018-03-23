using SharedKernel.Repositories.Contracts;
using BNE.PessoaFisica.Domain.Command;
using BNE.PessoaFisica.Domain.Model;

namespace BNE.PessoaFisica.Domain.Repositories
{
    public interface IPlanoRepository : IBaseRepository<Plano>
    {
        Plano GetPlano(GetPlanoCommand command);
    }
}