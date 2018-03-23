using BNE.Data.Infrastructure;

namespace BNE.PessoaFisica.Data.Repositories
{

    public class SituacaoCurriculoRepository : RepositoryBase<Model.SituacaoCurriculo>, ISituacaoCurriculoRepository
    {
        public SituacaoCurriculoRepository(IDatabaseFactory databaseFactory) : base(databaseFactory) { }
    }

    public interface ISituacaoCurriculoRepository : IRepository<Model.SituacaoCurriculo> { }
}