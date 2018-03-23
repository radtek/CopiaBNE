using System.Data.Entity;
using BNE.PessoaJuridica.Domain.Model;
using BNE.PessoaJuridica.Domain.Repositories;
using CrossCutting.Infrastructure.Repository;

namespace BNE.PessoaJuridica.Data.Repositories
{
    public class PerfilRepository : BaseRepository<Perfil, DbContext>, IPerfilRepository
    {
        public PerfilRepository(DbContext dataContext) : base(dataContext)
        {
        }
    }
}