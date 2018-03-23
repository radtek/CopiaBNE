using BNE.Data.Infrastructure;

namespace BNE.Global.Data.Repositories
{
    public class DeficienciaRepository : RepositoryBase<Model.DeficienciaGlobal>, IDeficienciaRepository
    {
        public DeficienciaRepository(IDatabaseFactory databaseFactory) : base(databaseFactory) { }
    }

    public interface IDeficienciaRepository : IRepository<Model.DeficienciaGlobal> { }
}