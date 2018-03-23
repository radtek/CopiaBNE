using System;
using System.Linq.Expressions;
using BNE.PessoaJuridica.Domain.Model;
using SharedKernel.Repositories.Contracts;

namespace BNE.PessoaJuridica.Domain.Repositories
{
    public interface ICNAERepository : IBaseRepository<CNAE>
    {
        CNAE Get(Expression<Func<CNAE, bool>> where);
    }
}