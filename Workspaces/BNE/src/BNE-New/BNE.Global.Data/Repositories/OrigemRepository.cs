using System.Data.Entity;
using BNE.Global.Model;
using CrossCutting.Infrastructure.Repository;
using SharedKernel.Repositories.Contracts;

namespace BNE.Global.Data.Repositories
{
    public class OrigemRepository : BaseRepository<OrigemGlobal, DbContext>, IOrigemRepository
    {
        public OrigemRepository(DbContext dataContext) : base(dataContext)
        {
        }
    }

    public interface IOrigemRepository : IBaseRepository<OrigemGlobal>
    {
    }
}