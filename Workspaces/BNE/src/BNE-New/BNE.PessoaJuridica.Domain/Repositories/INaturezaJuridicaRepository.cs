using System;
using System.Linq.Expressions;
using BNE.PessoaJuridica.Domain.Model;
using SharedKernel.Repositories.Contracts;

namespace BNE.PessoaJuridica.Domain.Repositories
{
    public interface INaturezaJuridicaRepository : IBaseRepository<NaturezaJuridica>
    {
        NaturezaJuridica Get(Expression<Func<NaturezaJuridica, bool>> where);
    }
}