using BNE.Data.Infrastructure;

namespace BNE.Global.Data.Repositories
{
    public class CategoriaCargoRepository : RepositoryBase<Model.CategoriaCargoGlobal>
    {
        public CategoriaCargoRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {

        }
    }

    public interface ICategoriaCargoRepository : IRepository<Model.CategoriaCargoGlobal> {  }
}