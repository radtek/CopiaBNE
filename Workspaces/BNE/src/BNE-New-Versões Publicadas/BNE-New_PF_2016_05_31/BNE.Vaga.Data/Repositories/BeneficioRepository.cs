using BNE.Data.Infrastructure;

namespace BNE.Vaga.Data.Repositories
{
    public class BeneficioRepository : RepositoryBase<Model.Beneficio>, IBeneficioRepository
    {
        public BeneficioRepository(IDatabaseFactory databaseFactory) : base(databaseFactory) { }
    }

    public interface IBeneficioRepository : IRepository<Model.Beneficio> { }
}