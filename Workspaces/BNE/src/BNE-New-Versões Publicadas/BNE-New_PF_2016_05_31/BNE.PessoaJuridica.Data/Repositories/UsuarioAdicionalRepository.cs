using BNE.Data.Infrastructure;

namespace BNE.PessoaJuridica.Data.Repositories
{
    public class UsuarioAdicionalRepository : RepositoryBase<Model.UsuarioAdicional>, IUsuarioAdicionalRepository
    {
        public UsuarioAdicionalRepository(IDatabaseFactory databaseFactory) : base(databaseFactory) { }
    }

    public interface IUsuarioAdicionalRepository : IRepository<Model.UsuarioAdicional> { }
}
