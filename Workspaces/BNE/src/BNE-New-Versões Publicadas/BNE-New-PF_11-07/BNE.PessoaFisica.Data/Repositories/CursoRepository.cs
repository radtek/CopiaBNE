using BNE.Data.Infrastructure;

namespace BNE.PessoaFisica.Data.Repositories
{
    public class CursoRepository : RepositoryBase<Model.Curso>, ICursoRepository
    {
        public CursoRepository(IDatabaseFactory databaseFactory) : base(databaseFactory) { }
    }

    public interface ICursoRepository : IRepository<Model.Curso> { }
}