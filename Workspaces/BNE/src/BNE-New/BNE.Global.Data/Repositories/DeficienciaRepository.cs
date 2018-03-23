using System.Data.Entity;
using BNE.Global.Model;
using CrossCutting.Infrastructure.Repository;
using SharedKernel.Repositories.Contracts;

namespace BNE.Global.Data.Repositories
{
    public class DeficienciaRepository : BaseRepository<DeficienciaGlobal, DbContext>, IDeficienciaRepository
    {
        public DeficienciaRepository(DbContext dataContext) : base(dataContext)
        {
        }
    }

    public interface IDeficienciaRepository : IBaseRepository<DeficienciaGlobal>
    {
    }
}