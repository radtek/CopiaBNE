using BNE.Data.Infrastructure;

namespace BNE.Mensagem.Data.Repositories
{
    public class SMSRepository : RepositoryBase<Model.SMS>, ISMSRepository
    {
        public SMSRepository(IDatabaseFactory databaseFactory) : base(databaseFactory) { }
    }

    public interface ISMSRepository : IRepository<Model.SMS> { }
}