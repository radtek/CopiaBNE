using BNE.Data.Infrastructure;

namespace BNE.PessoaFisica.Data.Repositories
{

    public class TelefoneCelularRepository : RepositoryBase<Model.TelefoneCelular>, ITelefoneCelularRepository
    {
        public TelefoneCelularRepository(IDatabaseFactory databaseFactory) : base(databaseFactory) { }
    }

    public interface ITelefoneCelularRepository : IRepository<Model.TelefoneCelular> { }
}