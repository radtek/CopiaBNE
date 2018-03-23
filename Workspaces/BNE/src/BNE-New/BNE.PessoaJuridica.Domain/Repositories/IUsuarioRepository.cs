using System;
using System.Linq.Expressions;
using BNE.PessoaJuridica.Domain.Model;
using SharedKernel.Repositories.Contracts;

namespace BNE.PessoaJuridica.Domain.Repositories
{
    public interface IUsuarioRepository : IBaseRepository<Usuario>
    {
        Usuario Get(Expression<Func<Usuario, bool>> where);
    }
}