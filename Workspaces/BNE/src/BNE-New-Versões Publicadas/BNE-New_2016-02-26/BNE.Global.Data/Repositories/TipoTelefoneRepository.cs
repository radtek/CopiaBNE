using BNE.Data.Infrastructure;

namespace BNE.Global.Data.Repositories
{
    public class TipoTelefoneRepository : RepositoryBase<Model.TipoTelefoneGlobal>, ITipoTelefoneRepository
    {
        public TipoTelefoneRepository(IDatabaseFactory databaseFactory) : base(databaseFactory) { }
    }

    public interface ITipoTelefoneRepository : IRepository<Model.TipoTelefoneGlobal> { }
}
