using System;
using System.Linq.Expressions;
using BNE.PessoaJuridica.Domain.Model;
using SharedKernel.Repositories.Contracts;

namespace BNE.PessoaJuridica.Domain.Repositories
{
    public interface IUsuarioPessoaJuridicaRepository : IBaseRepository<UsuarioPessoaJuridica>
    {
        UsuarioPessoaJuridica Get(Expression<Func<UsuarioPessoaJuridica, bool>> where);
    }
}