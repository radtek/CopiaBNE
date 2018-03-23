using BNE.ExceptionLog.Data.Infrastructure;

namespace BNE.ExceptionLog.Data.Repositories
{
    public class InformationRepository : RepositoryBase<Model.Information>, IInformationRepository
    {
        public InformationRepository(IDatabaseFactory databaseFactory) : base(databaseFactory) { }
    }

    public interface IInformationRepository : IRepository<Model.Information> { }
}
