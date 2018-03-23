using BNE.Data.Infrastructure;
using System.Data.Entity;

namespace APIGateway.Data.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDatabaseFactory _databaseFactory;
        private DbContext _dataContext;

        public UnitOfWork(IDatabaseFactory databaseFactory)
        {
            this._databaseFactory = databaseFactory;
        }

        protected APIGatewayContext DataContext
        {
            get { return (APIGatewayContext)(_dataContext ?? (_dataContext = _databaseFactory.Get())); }
        }

        public void Commit()
        {
            DataContext.Commit();
        }
    }

}
