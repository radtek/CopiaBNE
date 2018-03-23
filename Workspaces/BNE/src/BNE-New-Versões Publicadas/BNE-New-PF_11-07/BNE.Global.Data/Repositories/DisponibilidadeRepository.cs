using BNE.Data.Infrastructure;

namespace BNE.Global.Data.Repositories
{
    public class DisponibilidadeRepository : RepositoryBase<Model.DisponibilidadeGlobal>, IDisponibilidadeRepository
    {
        public DisponibilidadeRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {

        }
    }

    public interface IDisponibilidadeRepository : IRepository<Model.DisponibilidadeGlobal> { }
}