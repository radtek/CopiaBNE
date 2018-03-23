using MongoDB.Driver;

namespace BNE.ExceptionLog.Data.Infrastructure
{
    public interface IDatabaseFactory
    {
        IMongoDatabase Get();
    }
}
