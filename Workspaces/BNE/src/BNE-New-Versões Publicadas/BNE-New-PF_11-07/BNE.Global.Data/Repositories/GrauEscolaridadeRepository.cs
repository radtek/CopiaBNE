using BNE.Data.Infrastructure;

namespace BNE.Global.Data.Repositories
{
    public class GrauEscolaridadeRepository : RepositoryBase<Model.GrauEscolaridadeGlobal>, IGrauEscolaridadeRepository
    {
        public GrauEscolaridadeRepository(IDatabaseFactory databaseFactory) : base(databaseFactory) { }
    }

    public interface IGrauEscolaridadeRepository : IRepository<Model.GrauEscolaridadeGlobal> { }
}