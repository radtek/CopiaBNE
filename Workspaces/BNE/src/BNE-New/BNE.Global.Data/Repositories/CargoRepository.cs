using System.Data.Entity;
using BNE.Global.Model;
using CrossCutting.Infrastructure.Repository;
using SharedKernel.Repositories.Contracts;

namespace BNE.Global.Data.Repositories
{
    public class CargoRepository : BaseRepository<CargoGlobal, DbContext>, ICargoRepository
    {
        public CargoRepository(DbContext dataContext) : base(dataContext)
        {
        }
    }

    public interface ICargoRepository : IBaseRepository<CargoGlobal>
    {
    }
}