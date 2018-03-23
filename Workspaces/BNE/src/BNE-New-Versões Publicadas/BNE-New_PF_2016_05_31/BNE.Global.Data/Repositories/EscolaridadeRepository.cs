using BNE.Data.Infrastructure;

namespace BNE.Global.Data.Repositories
{
    public class EscolaridadeRepository : RepositoryBase<Model.EscolaridadeGlobal>, IEscolaridadeRepository    
    {
        public EscolaridadeRepository(IDatabaseFactory databaseFactory) : base(databaseFactory) { }
    }

    public interface IEscolaridadeRepository : IRepository<Model.EscolaridadeGlobal>{}
}