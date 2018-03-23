using SharedKernel.Repositories.Contracts;
using BNE.PessoaFisica.Domain.Model;

namespace BNE.PessoaFisica.Domain.Repositories
{
    public interface ICurriculoParametroRepository : IBaseRepository<CurriculoParametro>
    {
        CurriculoParametro GetByIdCurriculo(int idCurriculo);
    }
}