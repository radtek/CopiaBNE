using BNE.Data.Infrastructure;

namespace BNE.Global.Data.Repositories
{
    public class TipoOrigemRepository : RepositoryBase<Model.TipoOrigemGlobal>, ITipoOrigemRepository
    {
        public TipoOrigemRepository(IDatabaseFactory databaseFactory) : base(databaseFactory) { }
    }

    public interface ITipoOrigemRepository : IRepository<Model.TipoOrigemGlobal> { }
}