using BNE.Data.Infrastructure;

namespace BNE.PessoaFisica.Data.Repositories
{

    public class CurriculoOrigemRepository : RepositoryBase<Model.CurriculoOrigem>, ICurriculoOrigemRepository
    {
        public CurriculoOrigemRepository(IDatabaseFactory databaseFactory) : base(databaseFactory) { }
    }

    public interface ICurriculoOrigemRepository : IRepository<Model.CurriculoOrigem> { }
}