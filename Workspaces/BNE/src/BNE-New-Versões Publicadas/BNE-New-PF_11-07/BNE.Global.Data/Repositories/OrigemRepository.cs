using BNE.Data.Infrastructure;

namespace BNE.Global.Data.Repositories
{

    public class OrigemRepository : RepositoryBase<Model.OrigemGlobal>, IOrigemRepository
    {
        public OrigemRepository(IDatabaseFactory databaseFactory) : base(databaseFactory) { }
    }

    public interface IOrigemRepository : IRepository<Model.OrigemGlobal> { }
}