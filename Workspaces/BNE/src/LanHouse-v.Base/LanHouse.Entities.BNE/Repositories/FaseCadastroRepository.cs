using LanHouse.Entities.BNE.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanHouse.Entities.BNE.Repositories
{
    public class FaseCadastroRepository : RepositoryBase<LAN_Fase_Cadastro>, IFaseCadastroRepository
    {
        public FaseCadastroRepository(DatabaseFactory dbFactory)
            : base(dbFactory)
        {

        }
    }

    public interface IFaseCadastroRepository : IRepository<LAN_Fase_Cadastro>
    {

    }
}
