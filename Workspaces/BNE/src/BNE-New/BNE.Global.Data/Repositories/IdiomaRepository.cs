using System.Data.Entity;
using BNE.Global.Model;
using CrossCutting.Infrastructure.Repository;
using SharedKernel.Repositories.Contracts;

namespace BNE.Global.Data.Repositories
{
    public class IdiomaRepository : BaseRepository<IdiomaGlobal, DbContext>, IIdiomaRepository
    {
        public IdiomaRepository(DbContext dataContext) : base(dataContext)
        {
        }
    }

    public interface IIdiomaRepository : IBaseRepository<IdiomaGlobal>
    {
    }
}