using BNE.Data.Infrastructure;

namespace BNE.PessoaFisica.Data.Repositories
{
    public class PreCurriculoRepository : RepositoryBase<Model.PreCurriculo>, IPreCurriculoRepository
    {
        public PreCurriculoRepository(IDatabaseFactory databaseFactory) : base(databaseFactory) { }
    }

    public interface IPreCurriculoRepository : IRepository<Model.PreCurriculo> { }
}