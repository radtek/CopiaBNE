using BNE.Data.Infrastructure;

namespace BNE.Global.Data.Repositories
{
    public class BairroRepository : RepositoryBase<Model.Bairro>, IBairroRepository
    {
        public BairroRepository(IDatabaseFactory databaseFactory) : base(databaseFactory) { }
    }

    public interface IBairroRepository : IRepository<Model.Bairro> { }
}
