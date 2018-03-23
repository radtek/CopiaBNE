using System;
using System.Linq.Expressions;
using SharedKernel.Repositories.Contracts;

namespace BNE.PessoaJuridica.Domain.Repositories
{
    public interface IPessoaJuridicaRepository : IBaseRepository<Model.PessoaJuridica>
    {
        bool ExisteCNPJ(decimal cnpj);
        Model.PessoaJuridica Get(Expression<Func<Model.PessoaJuridica, bool>> where);
    }
}