using BNE.Data.Infrastructure;

namespace BNE.PessoaFisica.Data.Repositories
{
    public class InstituicaoEnsinoRepository : RepositoryBase<Model.InstituicaoEnsino>, IInstituicaoEnsinoRepository
    {
        public InstituicaoEnsinoRepository(IDatabaseFactory databaseFactory) : base(databaseFactory) { }
    }

    public interface IInstituicaoEnsinoRepository : IRepository<Model.InstituicaoEnsino> { }
}