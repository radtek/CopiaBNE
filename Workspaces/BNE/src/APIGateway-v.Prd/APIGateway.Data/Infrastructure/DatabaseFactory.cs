using BNE.Data.Infrastructure;
using System.Data.Entity;

namespace APIGateway.Data.Infrastructure
{
    public class DatabaseFactory : Disposable, IDatabaseFactory
    {

        private DbContext _dataContext;

        #region IDatabaseFactory members
        public DbContext Get()
        {
            return _dataContext ?? (_dataContext = new APIGatewayContext());
        }
        #endregion

        #region Disposable members
        protected override void DisposeCore()
        {
            if (_dataContext != null)
                _dataContext.Dispose();
        }
        #endregion

    }
}
