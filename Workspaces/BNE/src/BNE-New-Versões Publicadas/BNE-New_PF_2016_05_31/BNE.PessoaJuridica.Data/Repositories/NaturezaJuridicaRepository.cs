using BNE.Data.Infrastructure;

namespace BNE.PessoaJuridica.Data.Repositories
{
    public class NaturezaJuridicaRepository : RepositoryBase<Model.NaturezaJuridica>, INaturezaJuridicaRepository
    {
        public NaturezaJuridicaRepository(IDatabaseFactory databaseFactory) : base(databaseFactory) { }
    }

    public interface INaturezaJuridicaRepository : IRepository<Model.NaturezaJuridica> { }
}