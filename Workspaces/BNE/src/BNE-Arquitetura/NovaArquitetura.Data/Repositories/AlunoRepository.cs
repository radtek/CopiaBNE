using NovaArquitetura.Data.Infrastructure;
using NovaArquitetura.Entities;

namespace NovaArquitetura.Data.Repositories
{
    public class AlunoRepository : RepositoryBase<Aluno>, IAlunoRepository
    {
        public AlunoRepository(DatabaseFactory dbFactory)
            : base(dbFactory)
        {

        }
    }

    public interface IAlunoRepository : IRepository<Aluno>
    {

    }
}
