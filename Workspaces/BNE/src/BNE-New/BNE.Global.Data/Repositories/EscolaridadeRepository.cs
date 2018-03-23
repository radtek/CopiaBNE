using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using BNE.Global.Model;
using CrossCutting.Infrastructure.Repository;
using SharedKernel.Repositories.Contracts;

namespace BNE.Global.Data.Repositories
{
    public class EscolaridadeRepository : BaseRepository<EscolaridadeGlobal, DbContext>, IEscolaridadeRepository
    {
        public EscolaridadeRepository(DbContext dataContext) : base(dataContext)
        {
        }

        public IQueryable<EscolaridadeGlobal> GetMany(Expression<Func<EscolaridadeGlobal, bool>> where, params Expression<Func<EscolaridadeGlobal, object>>[] includes)
        {
            var query = DbSet.Where(where).AsQueryable();
            if (includes != null)
                query = includes.Aggregate(query, (current, include) => current.Include(include));

            return query;
        }
    }

    public interface IEscolaridadeRepository : IBaseRepository<EscolaridadeGlobal>
    {
        IQueryable<EscolaridadeGlobal> GetMany(Expression<Func<EscolaridadeGlobal, bool>> where, params Expression<Func<EscolaridadeGlobal, object>>[] includes);
    }
}