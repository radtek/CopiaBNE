using System.Configuration;
using MongoDB.Driver;

namespace BNE.ExceptionLog.Data.Infrastructure
{
    public class DatabaseFactory : IDatabaseFactory
    {

        private IMongoDatabase _dataContext;

        #region IDatabaseFactory members
        public IMongoDatabase Get()
        {
            var client = new MongoClient(ConfigurationManager.ConnectionStrings["MongoDB"].ConnectionString);

            return _dataContext ?? (_dataContext = client.GetDatabase("ExceptionLog"));
        }
        #endregion

    }
}
