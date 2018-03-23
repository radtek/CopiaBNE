using BNE.Dashboard.Data.Infrastructure;
using BNE.Dashboard.Entities;

namespace BNE.Dashboard.Data.Repositories
{
    public class WatcherRepository : RepositoryBase<Watcher>, IWatcherRepository
    {
        public WatcherRepository(IDatabaseFactory dbFactory)
            : base(dbFactory)
        {

        }
    }

    public interface IWatcherRepository : IRepository<Watcher>
    {

    }
}
