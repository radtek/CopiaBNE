using BNE.Data.Infrastructure;
using System.Data.Entity;

namespace BNE.Vaga.Data.Infrastructure
{
    public class UnitOfWorks : IUnitOfWork
    {
        private readonly IDatabaseFactory _databaseFactory;
        private DbContext _dataContext;

        public UnitOfWorks(IDatabaseFactory databaseFactory)
        {
            this._databaseFactory = databaseFactory;
        }

        protected VagaContext DataContext
        {
            get { return (VagaContext)(_dataContext ?? (_dataContext = _databaseFactory.Get())); }
        }

        public void Commit()
        {
            DataContext.Commit();
        }
    }
}
