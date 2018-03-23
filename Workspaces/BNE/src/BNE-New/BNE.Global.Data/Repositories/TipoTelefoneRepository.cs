using System.Data.Entity;
using BNE.Global.Model;
using CrossCutting.Infrastructure.Repository;
using SharedKernel.Repositories.Contracts;

namespace BNE.Global.Data.Repositories
{
    public class TipoTelefoneRepository : BaseRepository<TipoTelefoneGlobal, DbContext>, ITipoTelefoneRepository
    {
        public TipoTelefoneRepository(DbContext dataContext) : base(dataContext)
        {
        }
    }

    public interface ITipoTelefoneRepository : IBaseRepository<TipoTelefoneGlobal>
    {
    }
}