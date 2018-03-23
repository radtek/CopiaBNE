using System.Data.Entity;
using BNE.Data.Infrastructure;

namespace BNE.Mensagem.Data.Infrastructure
{
    public class DatabaseFactory : Disposable, IDatabaseFactory
    {

        private DbContext _dataContext;

        #region IDatabaseFactory members
        public DbContext Get()
        {
            return _dataContext ?? (_dataContext = new MensagemContext());
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