using System.Data.Entity;
using BNE.PessoaFisica.Domain.Model;
using BNE.PessoaFisica.Domain.Repositories;
using CrossCutting.Infrastructure.Repository;

namespace BNE.PessoaFisica.Data.Repositories
{
    public class SituacaoCurriculoRepository : BaseRepository<SituacaoCurriculo, DbContext>, ISituacaoCurriculoRepository
    {
        public SituacaoCurriculoRepository(DbContext dataContext) : base(dataContext)
        {
        }
    }
}