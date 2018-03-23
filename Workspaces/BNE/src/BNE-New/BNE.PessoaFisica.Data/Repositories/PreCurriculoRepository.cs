using System.Data.Entity;
using BNE.PessoaFisica.Domain.Model;
using BNE.PessoaFisica.Domain.Repositories;
using CrossCutting.Infrastructure.Repository;

namespace BNE.PessoaFisica.Data.Repositories
{
    public class PreCurriculoRepository : BaseRepository<PreCurriculo, DbContext>, IPreCurriculoRepository
    {
        public PreCurriculoRepository(DbContext dataContext) : base(dataContext)
        {
        }
    }
}