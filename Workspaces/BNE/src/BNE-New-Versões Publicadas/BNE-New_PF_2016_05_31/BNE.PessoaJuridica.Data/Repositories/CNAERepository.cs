using BNE.Data.Infrastructure;

namespace BNE.PessoaJuridica.Data.Repositories
{
    public class CNAERepository : RepositoryBase<Model.CNAE>, ICNAERepository
    {
        public CNAERepository(IDatabaseFactory databaseFactory) : base(databaseFactory) { }
    }

    public interface ICNAERepository : IRepository<Model.CNAE> { }
}