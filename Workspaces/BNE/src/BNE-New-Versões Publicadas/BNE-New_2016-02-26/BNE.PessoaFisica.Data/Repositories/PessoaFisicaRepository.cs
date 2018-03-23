using BNE.Data.Infrastructure;

namespace BNE.PessoaFisica.Data.Repositories
{
    public class PessoaFisicaRepository : RepositoryBase<Model.PessoaFisica>, IPessoaFisicaRepository
    {
        public PessoaFisicaRepository(IDatabaseFactory databaseFactory) : base(databaseFactory) { }
    }

    public interface IPessoaFisicaRepository : IRepository<Model.PessoaFisica> { }
}