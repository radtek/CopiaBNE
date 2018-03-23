using LanHouse.Entities.BNE.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanHouse.Entities.BNE.Repositories
{
    public class AreaBNERepository : RepositoryBase<TAB_Area_BNE>, IAreaBNERepository
    {
        public AreaBNERepository(DatabaseFactory dbFactory):base(dbFactory)
        {
        }
    }

    public interface IAreaBNERepository : IRepository<TAB_Area_BNE>
    {

    }
}
