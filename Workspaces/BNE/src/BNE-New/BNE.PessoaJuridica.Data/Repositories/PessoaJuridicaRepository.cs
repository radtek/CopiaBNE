using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using BNE.PessoaJuridica.Domain.Repositories;
using CrossCutting.Infrastructure.Repository;

namespace BNE.PessoaJuridica.Data.Repositories
{
    public class PessoaJuridicaRepository : BaseRepository<Domain.Model.PessoaJuridica, DbContext>, IPessoaJuridicaRepository
    {
        public PessoaJuridicaRepository(DbContext dataContext) : base(dataContext)
        {
        }

        public bool ExisteCNPJ(decimal cnpj)
        {
            return DbSet.Any(n => n.CNPJ == cnpj);
        }

        public Domain.Model.PessoaJuridica Get(Expression<Func<Domain.Model.PessoaJuridica, bool>> where)
        {
            return DbSet.FirstOrDefault(where);
        }
    }
}