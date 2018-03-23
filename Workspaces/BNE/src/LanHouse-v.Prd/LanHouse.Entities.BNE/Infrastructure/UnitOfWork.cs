using LanHouse.Entities.BNE;
namespace LanHouse.Entities.BNE.Infrastructure
{
    public class UnitOfWork
    {
        private LanEntities dbContext;
        private readonly DatabaseFactory _dbFactory;

        protected LanEntities DbContext
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
