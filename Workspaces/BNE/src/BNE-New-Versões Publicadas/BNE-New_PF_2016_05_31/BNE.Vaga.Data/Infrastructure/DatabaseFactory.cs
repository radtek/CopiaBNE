using BNE.Data.Infrastructure;
using System.Data.Entity;

namespace BNE.Vaga.Data.Infrastructure
{
    public class DatabaseFactory : Disposable, IDatabaseFactory
    {
        private DbContext _dataContext;

        #region IDatabaseFactory member
        public DbContext Get()
        {
            return _dataContext ?? (_dataContext = new VagaContext());
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
