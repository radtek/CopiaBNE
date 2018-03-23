using BNE.Data.Infrastructure;

namespace BNE.PessoaJuridica.Data.Repositories
{
    public class UsuarioRepository : RepositoryBase<Model.Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(IDatabaseFactory databaseFactory) : base(databaseFactory) { }
    }

    public interface IUsuarioRepository : IRepository<Model.Usuario> { }
}
