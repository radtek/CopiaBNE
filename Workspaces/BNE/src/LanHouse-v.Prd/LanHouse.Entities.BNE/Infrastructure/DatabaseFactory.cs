using LanHouse.Entities.BNE;
namespace LanHouse.Entities.BNE.Infrastructure
{
    public class DatabaseFactory : Disposable
    {
        private LanEntities _dataContext;

        public LanEntities Get()
        {
            return _dataContext ?? (_dataContext = new LanEntities());
        }

        protected override void DisposeCore()
        {
            if (_dataContext != null)
                _dataContext.Dispose();
        }
    }
}
