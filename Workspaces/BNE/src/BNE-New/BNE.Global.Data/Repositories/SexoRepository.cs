using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using BNE.Global.Model;
using CrossCutting.Infrastructure.Repository;
using SharedKernel.Repositories.Contracts;

namespace BNE.Global.Data.Repositories
{
    public class SexoRepository : BaseRepository<Sexo, DbContext>, ISexoRepository
    {
        public SexoRepository(DbContext dataContext) : base(dataContext)
        {
        }

        public Sexo GetByChar(string sexo)
        {
            return Get(n => n.Sigla == sexo);
        }

        public Sexo Get(Expression<Func<Sexo, bool>> where)
        {
            return DbSet.Where(where).FirstOrDefault();
        }
    }

    public interface ISexoRepository : IBaseRepository<Sexo>
    {
        Sexo GetByChar(string sexo);
        Sexo Get(Expression<Func<Sexo, bool>> where);
    }
}