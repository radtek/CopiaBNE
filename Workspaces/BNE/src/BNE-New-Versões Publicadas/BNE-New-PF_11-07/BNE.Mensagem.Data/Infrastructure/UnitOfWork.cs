using System.Data.Entity;
using BNE.Data.Infrastructure;

namespace BNE.Mensagem.Data.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDatabaseFactory _databaseFactory;
        private DbContext _dataContext;

        public UnitOfWork(IDatabaseFactory databaseFactory)
        {
            this._databaseFactory = databaseFactory;
        }

        protected MensagemContext DataContext
        {
            get { return (MensagemContext)(_dataContext ?? (_dataContext = _databaseFactory.Get())); }
        }

        public void Commit()
        {
            DataContext.Commit();
        }
    }
}
