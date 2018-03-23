using System.Data.Entity;
using BNE.Data.Infrastructure;

namespace BNE.PessoaJuridica.Data.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDatabaseFactory _databaseFactory;
        private DbContext _dataContext;

        public UnitOfWork(IDatabaseFactory databaseFactory)
        {
            this._databaseFactory = databaseFactory;
        }

        protected PessoaJuridicaContext DataContext
        {
            get { return (PessoaJuridicaContext)(_dataContext ?? (_dataContext = _databaseFactory.Get())); }
        }

        public void Commit()
        {
            DataContext.Commit();
        }
    }
}
