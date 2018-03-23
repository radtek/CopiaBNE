using BNE.Data.Infrastructure;

namespace BNE.PessoaFisica.Data.Repositories
{

    public class CurriculoRepository : RepositoryBase<Model.Curriculo>, ICurriculoRepository
    {
        public CurriculoRepository(IDatabaseFactory databaseFactory) : base(databaseFactory) { }
    }

    public interface ICurriculoRepository : IRepository<Model.Curriculo> { }
}