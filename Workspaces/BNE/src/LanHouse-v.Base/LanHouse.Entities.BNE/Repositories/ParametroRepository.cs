using LanHouse.Entities.BNE.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanHouse.Entities.BNE.Repositories
{
    public class ParametroRepository : RepositoryBase<TAB_Parametro>, IParametroRepository
    {
        public ParametroRepository(DatabaseFactory dbFactory)
            : base(dbFactory)
        {

        }
    }

    public interface IParametroRepository : IRepository<TAB_Parametro>
    {

    }
}
