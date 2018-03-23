using System.Data.Entity;
using BNE.Global.Model;
using CrossCutting.Infrastructure.Repository;
using SharedKernel.Repositories.Contracts;

namespace BNE.Global.Data.Repositories
{
    public class DisponibilidadeRepository : BaseRepository<DisponibilidadeGlobal, DbContext>, IDisponibilidadeRepository
    {
        public DisponibilidadeRepository(DbContext dataContext) : base(dataContext)
        {
        }
    }

    public interface IDisponibilidadeRepository : IBaseRepository<DisponibilidadeGlobal>
    {
    }
}