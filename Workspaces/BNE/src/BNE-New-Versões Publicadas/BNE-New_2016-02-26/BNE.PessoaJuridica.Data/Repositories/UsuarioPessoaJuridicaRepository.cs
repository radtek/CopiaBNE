using BNE.Data.Infrastructure;

namespace BNE.PessoaJuridica.Data.Repositories
{
    public class UsuarioPessoaJuridicaRepository : RepositoryBase<Model.UsuarioPessoaJuridica>, IUsuarioPessoaJuridicaRepository
    {
        public UsuarioPessoaJuridicaRepository(IDatabaseFactory databaseFactory) : base(databaseFactory) { }
    }

    public interface IUsuarioPessoaJuridicaRepository : IRepository<Model.UsuarioPessoaJuridica> { }
}
