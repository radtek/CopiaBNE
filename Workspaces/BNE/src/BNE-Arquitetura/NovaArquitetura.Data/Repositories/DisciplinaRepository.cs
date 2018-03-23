using NovaArquitetura.Data.Infrastructure;
using NovaArquitetura.Entities;

namespace NovaArquitetura.Data.Repositories
{
    public class DisciplinaRepository : RepositoryBase<Disciplina>, IDisciplinaRepository
    {
        public DisciplinaRepository(DatabaseFactory dbFactory) : base(dbFactory)
        {

        }
    }

    public interface IDisciplinaRepository : IRepository<Disciplina>
    {

    }
}
