using BNE.Data.Infrastructure;

namespace BNE.Mensagem.Data.Repositories
{
    public class SistemaRepository : RepositoryBase<Model.Sistema>, ISistemaRepository
    {
        public SistemaRepository(IDatabaseFactory databaseFactory) : base(databaseFactory) { }
    }

    public interface ISistemaRepository : IRepository<Model.Sistema> { }
}
