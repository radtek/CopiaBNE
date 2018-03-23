using BNE.Data.Infrastructure;

namespace BNE.PessoaFisica.Data.Repositories
{
    public class ExperienciaProfissionalRepository : RepositoryBase<Model.ExperienciaProfissional>, IExperienciaProfissionalRepository
    {
        public ExperienciaProfissionalRepository(IDatabaseFactory databaseFactory) : base(databaseFactory) { }
    }

    public interface IExperienciaProfissionalRepository : IRepository<Model.ExperienciaProfissional> { }
}