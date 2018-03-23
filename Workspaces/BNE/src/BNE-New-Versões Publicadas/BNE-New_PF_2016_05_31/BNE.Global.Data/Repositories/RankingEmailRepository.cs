using BNE.Data.Infrastructure;

namespace BNE.Global.Data.Repositories
{
    public class RankingEmailRepository : RepositoryBase<Model.RankingEmail>, IRankingEmailRepository
    {
        public RankingEmailRepository(IDatabaseFactory databaseFactory) : base(databaseFactory) { }
    }

    public interface IRankingEmailRepository : IRepository<Model.RankingEmail> { }
}