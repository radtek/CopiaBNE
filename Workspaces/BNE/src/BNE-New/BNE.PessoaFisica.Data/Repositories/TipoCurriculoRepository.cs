using System.Data.Entity;
using BNE.PessoaFisica.Domain.Model;
using BNE.PessoaFisica.Domain.Repositories;
using CrossCutting.Infrastructure.Repository;

namespace BNE.PessoaFisica.Data.Repositories
{
    public class TipoCurriculoRepository : BaseRepository<TipoCurriculo, DbContext>, ITipoCurriculoRepository
    {
        public TipoCurriculoRepository(DbContext dataContext) : base(dataContext)
        {
        }
    }
}