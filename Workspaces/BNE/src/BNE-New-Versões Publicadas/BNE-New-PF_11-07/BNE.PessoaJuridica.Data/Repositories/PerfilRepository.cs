using BNE.Data.Infrastructure;

namespace BNE.PessoaJuridica.Data.Repositories
{
    public class PerfilRepository : RepositoryBase<Model.Perfil>, IPerfilRepository
    {
        public PerfilRepository(IDatabaseFactory databaseFactory) : base(databaseFactory) { }
    }

    public interface IPerfilRepository : IRepository<Model.Perfil> { }
}