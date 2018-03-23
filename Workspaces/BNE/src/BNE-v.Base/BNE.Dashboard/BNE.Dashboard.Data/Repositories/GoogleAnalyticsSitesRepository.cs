using BNE.Dashboard.Data.Infrastructure;
using BNE.Dashboard.Entities;

namespace BNE.Dashboard.Data.Repositories
{
    public class GoogleAnalyticsSitesRepository : RepositoryBase<GoogleAnalyticsSites>, IGoogleAnalyticsSitesRepository
    {
        public GoogleAnalyticsSitesRepository(IDatabaseFactory dbFactory)
            : base(dbFactory)
        {

        }
    }

    public interface IGoogleAnalyticsSitesRepository : IRepository<GoogleAnalyticsSites>
    {

    }
}
