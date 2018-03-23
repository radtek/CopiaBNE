
namespace NovaArquitetura.Data.Infrastructure
{
    public class UnitOfWork
    {
        private NovaArquiteturaEntities dbContext;
        private readonly DatabaseFactory _dbFactory;
        protected NovaArquiteturaEntities DbContext
        {
            get
            {
                return dbContext ?? _dbFactory.Get();
            }
        }

        public UnitOfWork(DatabaseFactory dbFactory)
        {
            this._dbFactory = dbFactory;
        }

        public void SaveChanges()
        {
            DbContext.SaveChanges();
        }
    }
}
