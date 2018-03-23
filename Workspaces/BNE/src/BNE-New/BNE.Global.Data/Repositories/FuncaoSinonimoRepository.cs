using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using BNE.Global.Model;
using CrossCutting.Infrastructure.Repository;
using SharedKernel.Repositories.Contracts;

namespace BNE.Global.Data.Repositories
{
    public class FuncaoSinonimoRepository : BaseRepository<FuncaoSinonimo, DbContext>, IFuncaoSinonimoRepository
    {
        public FuncaoSinonimoRepository(DbContext dataContext) : base(dataContext)
        {
        }

        public FuncaoSinonimo GetByNome(string funcao)
        {
            return Get(n => n.NomeSinonimo == funcao);
        }

        public FuncaoSinonimo Get(Expression<Func<FuncaoSinonimo, bool>> where)
        {
            return DbSet.Where(where).FirstOrDefault();
        }
    }

    public interface IFuncaoSinonimoRepository : IBaseRepository<FuncaoSinonimo>
    {
        FuncaoSinonimo GetByNome(string funcao);
        FuncaoSinonimo Get(Expression<Func<FuncaoSinonimo, bool>> where);
    }
}