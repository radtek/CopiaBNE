using System.Data.Entity;
using BNE.Global.Model;
using CrossCutting.Infrastructure.Repository;
using SharedKernel.Repositories.Contracts;

namespace BNE.Global.Data.Repositories
{
    public class GrauEscolaridadeRepository : BaseRepository<GrauEscolaridadeGlobal, DbContext>, IGrauEscolaridadeRepository
    {
        public GrauEscolaridadeRepository(DbContext dataContext) : base(dataContext)
        {
        }
    }

    public interface IGrauEscolaridadeRepository : IBaseRepository<GrauEscolaridadeGlobal>
    {
    }
}