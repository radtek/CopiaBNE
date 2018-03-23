using BNE.Data.Infrastructure;

namespace BNE.Mensagem.Data.Repositories
{
    public class StatusRepository : RepositoryBase<Model.Status>, IStatusRepository
    {
        public StatusRepository(IDatabaseFactory databaseFactory) : base(databaseFactory) { }
    }

    public interface IStatusRepository : IRepository<Model.Status> { }
}
