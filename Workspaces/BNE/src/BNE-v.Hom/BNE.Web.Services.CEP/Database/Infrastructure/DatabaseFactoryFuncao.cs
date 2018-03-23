using System.Data.Entity;

namespace BNE.Web.Services.Solr.Database.Infrastructure
{
    public class DatabaseFactoryFuncao : Disposable, IDatabaseFactory
    {

        private DbContext _dataContext;

        #region IDatabaseFactory members
        public DbContext Get()
        {
            return _dataContext ?? (_dataContext = new FuncaoContext());
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