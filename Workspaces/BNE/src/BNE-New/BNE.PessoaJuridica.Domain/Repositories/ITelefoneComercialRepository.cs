using System;
using System.Linq.Expressions;
using BNE.PessoaJuridica.Domain.Model;
using SharedKernel.Repositories.Contracts;

namespace BNE.PessoaJuridica.Domain.Repositories
{
    public interface ITelefoneComercialRepository : IBaseRepository<TelefoneComercial>
    {
        TelefoneComercial Get(Expression<Func<TelefoneComercial, bool>> where);
    }
}