namespace BNE.Dashboard.Data.Infrastructure
{
    public class DatabaseFactory : Disposable, IDatabaseFactory
    {
        private DashboardEntities _dataContext;
        public DashboardEntities Get()
        {
            return _dataContext ?? (_dataContext = new DashboardEntities());
        }
        protected override void DisposeCore()
        {
            if (_dataContext != null)
                _dataContext.Dispose();
        }
    }
}
