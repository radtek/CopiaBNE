using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using BNE.Global.Model;
using CrossCutting.Infrastructure.Repository;
using SharedKernel.Repositories.Contracts;

namespace BNE.Global.Data.Repositories
{
    public class BairroRepository : BaseRepository<Bairro, DbContext>, IBairroRepository
    {
        public BairroRepository(DbContext dataContext) : base(dataContext)
        {
        }

        public Bairro Get(Expression<Func<Bairro, bool>> exp)
        {
            return DbSet.Where(exp).FirstOrDefault();
        }
    }

    public interface IBairroRepository : IBaseRepository<Bairro>
    {
        Bairro Get(Expression<Func<Bairro, bool>> where);
    }
}