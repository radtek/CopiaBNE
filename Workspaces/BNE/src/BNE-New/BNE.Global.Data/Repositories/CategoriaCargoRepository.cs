using System.Data.Entity;
using BNE.Global.Model;
using CrossCutting.Infrastructure.Repository;
using SharedKernel.Repositories.Contracts;

namespace BNE.Global.Data.Repositories
{
    public class CategoriaCargoRepository : BaseRepository<CategoriaCargoGlobal, DbContext>, ICategoriaCargoRepository
    {
        public CategoriaCargoRepository(DbContext dataContext) : base(dataContext)
        {
        }
    }

    public interface ICategoriaCargoRepository : IBaseRepository<CategoriaCargoGlobal>
    {
    }
}