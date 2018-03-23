using BNE.Data.Infrastructure;

namespace BNE.Global.Data.Repositories
{
    public class FuncaoSinonimoRepository : RepositoryBase<Model.FuncaoSinonimo>, IFuncaoSinonimoRepository
    {
        public FuncaoSinonimoRepository(IDatabaseFactory databaseFactory) : base(databaseFactory) { }
    }

    public interface IFuncaoSinonimoRepository : IRepository<Model.FuncaoSinonimo> { }
}
