using System.Data.Entity;
using BNE.PessoaJuridica.Domain.Model;
using BNE.PessoaJuridica.Domain.Repositories;
using CrossCutting.Infrastructure.Repository;

namespace BNE.PessoaJuridica.Data.Repositories
{
    public class ParametroRepository : BaseRepository<Parametro, DbContext>, IParametroRepository
    {
        public ParametroRepository(DbContext dataContext) : base(dataContext)
        {
        }
    }
}