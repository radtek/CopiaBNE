using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using BNE.PessoaFisica.Domain.Model;
using BNE.PessoaFisica.Domain.Repositories;
using CrossCutting.Infrastructure.Repository;

namespace BNE.PessoaFisica.Data.Repositories
{
    public class CurriculoParametroRepository : BaseRepository<CurriculoParametro, DbContext>, ICurriculoParametroRepository
    {
        public CurriculoParametroRepository(DbContext dataContext) : base(dataContext)
        {
        }

        public CurriculoParametro GetByIdCurriculo(int idCurriculo)
        {
            return GetMany(p => p.Curriculo.Id == idCurriculo).FirstOrDefault();
        }

        public virtual IQueryable<CurriculoParametro> GetMany(Expression<Func<CurriculoParametro, bool>> where, params Expression<Func<CurriculoParametro, object>>[] includes)
        {
            var query = DbSet.Where(where).AsQueryable();
            if (includes != null)
                query = includes.Aggregate(query, (current, include) => current.Include(include));

            return query;
        }
    }
}