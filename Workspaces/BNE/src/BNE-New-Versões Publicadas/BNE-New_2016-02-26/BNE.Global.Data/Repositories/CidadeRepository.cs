using BNE.Data.Infrastructure;

namespace BNE.Global.Data.Repositories
{
    public class CidadeRepository : RepositoryBase<Model.Cidade>, ICidadeRepository
    {
        public CidadeRepository(IDatabaseFactory databaseFactory) : base(databaseFactory) { }
    }

    public interface ICidadeRepository : IRepository<Model.Cidade> { }
}
