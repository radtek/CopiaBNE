using BNE.Data.Infrastructure;

namespace BNE.Global.Data.Repositories
{
    public class CargoRepository : RepositoryBase<Model.CargoGlobal>
    {
        public CargoRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {

        }
    }

    public interface ICargoRepository : IRepository<Model.CargoGlobal> { }
}