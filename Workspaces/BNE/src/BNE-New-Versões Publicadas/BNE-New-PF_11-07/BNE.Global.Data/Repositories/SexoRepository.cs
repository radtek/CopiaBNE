using BNE.Data.Infrastructure;

namespace BNE.Global.Data.Repositories
{
    public class SexoRepository : RepositoryBase<Model.Sexo>, ISexoRepository
    {
        public SexoRepository(IDatabaseFactory databaseFactory) : base(databaseFactory) { }
    }

    public interface ISexoRepository : IRepository<Model.Sexo> { }
}