using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LanHouse.Entities.BNE.Infrastructure;


namespace LanHouse.Entities.BNE.Repositories
{
    public class CompanhiaRepository : RepositoryBase<LAN_Companhia>, ICompanhiaRepository
    {
        public CompanhiaRepository(DatabaseFactory dbFactory) : base(dbFactory)
        {
        }
    }

    public interface ICompanhiaRepository : IRepository<LAN_Companhia>
    { 
    }
}
