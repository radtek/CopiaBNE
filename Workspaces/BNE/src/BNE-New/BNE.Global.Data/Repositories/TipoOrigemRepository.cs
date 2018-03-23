using System.Data.Entity;
using BNE.Global.Model;
using CrossCutting.Infrastructure.Repository;
using SharedKernel.Repositories.Contracts;

namespace BNE.Global.Data.Repositories
{
    public class TipoOrigemRepository : BaseRepository<TipoOrigemGlobal, DbContext>, ITipoOrigemRepository
    {
        public TipoOrigemRepository(DbContext dataContext) : base(dataContext)
        {
        }
    }

    public interface ITipoOrigemRepository : IBaseRepository<TipoOrigemGlobal>
    {
    }
}