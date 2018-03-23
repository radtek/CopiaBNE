namespace BNE.Dashboard.Data.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private DashboardEntities _dbContext;
        private readonly IDatabaseFactory _dbFactory;
        protected DashboardEntities DbContext
        {
            get
            {
                return _dbContext ?? _dbFactory.Get();
            }
        }

        public UnitOfWork(IDatabaseFactory dbFactory)
        {
            this._dbFactory = dbFactory;
        }

        public void SaveChanges()
        {
            DbContext.SaveChanges();
        }
    }
}
