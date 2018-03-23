using BNE.Data.Infrastructure;

namespace BNE.PessoaFisica.Data.Repositories
{
    public class FormacaoRepository : RepositoryBase<Model.Formacao>, IFormacaoRepository
    {
        public FormacaoRepository(IDatabaseFactory databaseFactory) : base(databaseFactory) { }
    }

    public interface IFormacaoRepository : IRepository<Model.Formacao> { }
}