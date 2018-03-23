using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using BNE.Global.Model;
using CrossCutting.Infrastructure.Repository;
using SharedKernel.Repositories.Contracts;

namespace BNE.Global.Data.Repositories
{
    public class RankingEmailRepository : BaseRepository<RankingEmail, DbContext>, IRankingEmailRepository
    {
        public RankingEmailRepository(DbContext dataContext) : base(dataContext)
        {
        }

        public IQueryable<RankingEmail> GetMany(Expression<Func<RankingEmail, bool>> @where, params Expression<Func<RankingEmail, object>>[] includes)
        {
            var query = DbSet.Where(where).AsQueryable();
            if (includes != null)
                query = includes.Aggregate(query, (current, include) => current.Include(include));

            return query;
        }
    }

    public interface IRankingEmailRepository : IBaseRepository<RankingEmail>
    {
        IQueryable<RankingEmail> GetMany(Expression<Func<RankingEmail, bool>> where, params Expression<Func<RankingEmail, object>>[] includes);
    }
}