using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using BNE.PessoaJuridica.Domain.Model;
using BNE.PessoaJuridica.Domain.Repositories;
using CrossCutting.Infrastructure.Repository;

namespace BNE.PessoaJuridica.Data.Repositories
{
    public class NaturezaJuridicaRepository : BaseRepository<NaturezaJuridica, DbContext>, INaturezaJuridicaRepository
    {
        public NaturezaJuridicaRepository(DbContext dataContext) : base(dataContext)
        {
        }

        public NaturezaJuridica Get(Expression<Func<NaturezaJuridica, bool>> where)
        {
            return DbSet.FirstOrDefault(where);
        }
    }
}