using System.Data.Entity;
using BNE.PessoaFisica.Domain.Model;
using BNE.PessoaFisica.Domain.Repositories;
using CrossCutting.Infrastructure.Repository;

namespace BNE.PessoaFisica.Data.Repositories
{
    public class ParametroRepository : BaseRepository<Parametro, DbContext>, IParametroRepository
    {
        public ParametroRepository(DbContext dataContext) : base(dataContext)
        {
        }
    }
}