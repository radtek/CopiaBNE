using System;
using System.Linq.Expressions;
using BNE.PessoaJuridica.Domain.Model;
using SharedKernel.Repositories.Contracts;

namespace BNE.PessoaJuridica.Domain.Repositories
{
    public interface ITelefoneCelularRepository : IBaseRepository<TelefoneCelular>
    {
        TelefoneCelular Get(Expression<Func<TelefoneCelular, bool>> where);
    }
}