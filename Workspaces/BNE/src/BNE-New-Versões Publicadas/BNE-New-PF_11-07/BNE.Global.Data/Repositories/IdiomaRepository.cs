using BNE.Data.Infrastructure;

namespace BNE.Global.Data.Repositories
{
    public class IdiomaRepository : RepositoryBase<Model.IdiomaGlobal>, IIdiomaRepository
    {
        public IdiomaRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {

        }
    }

    public interface IIdiomaRepository : IRepository<Model.IdiomaGlobal> { }
}