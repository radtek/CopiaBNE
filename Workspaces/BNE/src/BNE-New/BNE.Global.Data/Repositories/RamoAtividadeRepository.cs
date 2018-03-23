using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using BNE.Global.Model;
using CrossCutting.Infrastructure.Repository;
using SharedKernel.Repositories.Contracts;

namespace BNE.Global.Data.Repositories
{
    public class RamoAtividadeRepository : BaseRepository<RamoAtividadeGlobal, DbContext>, IRamoAtividadeRepository
    {
        public RamoAtividadeRepository(DbContext dataContext) : base(dataContext)
        {
        }

        public IEnumerable<RamoAtividadeGlobal> GetAll()
        {
            return DbSet.ToList();
        }
    }

    public interface IRamoAtividadeRepository : IBaseRepository<RamoAtividadeGlobal>
    {
        IEnumerable<RamoAtividadeGlobal> GetAll();
    }
}