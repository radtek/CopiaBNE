using BNE.Data.Infrastructure;
using System.Data.Entity;

namespace BNE.PessoaFisica.Data.Infrastructure
{
    public class UnitOfWorks : IUnitOfWork
    {
        private readonly IDatabaseFactory _databaseFactory;
        private DbContext _dataContext;

        public UnitOfWorks(IDatabaseFactory databaseFactory)
        {
            this._databaseFactory = databaseFactory;
        }

        protected PessoaFisicaContext DataContext
        {
            get { return (PessoaFisicaContext)(_dataContext ?? (_dataContext = _databaseFactory.Get())); }
        }

        public void Commit()
        {
            DataContext.Commit();
        }
    }
}
