using BNE.ExceptionLog.Data.Infrastructure;

namespace BNE.ExceptionLog.Data.Repositories
{
    public class ErrorRepository : RepositoryBase<Model.Error>, IErrorRepository
    {
        public ErrorRepository(IDatabaseFactory databaseFactory) : base(databaseFactory) { }
    }

    public interface IErrorRepository : IRepository<Model.Error> { }
}
