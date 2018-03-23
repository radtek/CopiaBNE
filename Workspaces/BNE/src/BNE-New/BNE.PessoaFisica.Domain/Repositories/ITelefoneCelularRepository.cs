using System;
using SharedKernel.Repositories.Contracts;
using BNE.PessoaFisica.Domain.Model;

namespace BNE.PessoaFisica.Domain.Repositories
{
    public interface ITelefoneCelularRepository : IBaseRepository<TelefoneCelular>
    {
        TelefoneCelular Get(Func<TelefoneCelular, bool> where);
    }
}