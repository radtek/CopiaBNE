using System.Data.Entity;
using BNE.PessoaJuridica.Domain.Model;
using BNE.PessoaJuridica.Domain.Repositories;
using CrossCutting.Infrastructure.Repository;

namespace BNE.PessoaJuridica.Data.Repositories
{
    public class UsuarioAdicionalRepository : BaseRepository<UsuarioAdicional, DbContext>, IUsuarioAdicionalRepository
    {
        public UsuarioAdicionalRepository(DbContext dataContext) : base(dataContext)
        {
        }
    }
}