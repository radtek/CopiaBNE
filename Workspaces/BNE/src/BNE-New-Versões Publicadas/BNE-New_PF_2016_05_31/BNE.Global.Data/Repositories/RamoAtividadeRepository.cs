using BNE.Data.Infrastructure;

namespace BNE.Global.Data.Repositories
{
    public class RamoAtividadeRepository : RepositoryBase<Model.RamoAtividadeGlobal>, IRamoAtividadeRepository
    {
        public RamoAtividadeRepository(IDatabaseFactory databaseFactory) : base(databaseFactory) { }
    }

    public interface IRamoAtividadeRepository : IRepository<Model.RamoAtividadeGlobal> { }
}