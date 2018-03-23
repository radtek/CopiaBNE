using System.Data.Entity;
using BNE.PessoaFisica.Domain.Model;
using BNE.PessoaFisica.Domain.Repositories;
using CrossCutting.Infrastructure.Repository;

namespace BNE.PessoaFisica.Data.Repositories
{
    public class CurriculoOrigemRepository : BaseRepository<CurriculoOrigem, DbContext>, ICurriculoOrigemRepository
    {
        public CurriculoOrigemRepository(DbContext dataContext) : base(dataContext)
        {
        }
    }
}