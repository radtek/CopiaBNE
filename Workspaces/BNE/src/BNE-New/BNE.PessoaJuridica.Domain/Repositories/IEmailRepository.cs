using System;
using System.Linq.Expressions;
using BNE.PessoaJuridica.Domain.Model;
using SharedKernel.Repositories.Contracts;

namespace BNE.PessoaJuridica.Domain.Repositories
{
    public interface IEmailRepository : IBaseRepository<Email>
    {
        Email Get(Expression<Func<Email, bool>> where);
    }
}