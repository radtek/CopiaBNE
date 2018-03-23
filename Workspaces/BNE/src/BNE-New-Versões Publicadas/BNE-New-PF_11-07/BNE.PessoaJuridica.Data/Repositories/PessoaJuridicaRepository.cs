using BNE.Data.Infrastructure;

namespace BNE.PessoaJuridica.Data.Repositories
{
    public class PessoaJuridicaRepository : RepositoryBase<Model.PessoaJuridica>, IPessoaJuridicaRepository
    {
        public PessoaJuridicaRepository(IDatabaseFactory databaseFactory) : base(databaseFactory) { }
    }

    public interface IPessoaJuridicaRepository : IRepository<Model.PessoaJuridica> { }
}
