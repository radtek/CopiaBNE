using BNE.ExceptionLog.Data.Infrastructure;

namespace BNE.ExceptionLog.Data.Repositories
{
    public class WarningRepository : RepositoryBase<Model.Warning>, IWarningRepository
    {
        public WarningRepository(IDatabaseFactory databaseFactory) : base(databaseFactory) { }
    }

    public interface IWarningRepository : IRepository<Model.Warning> { }
}
