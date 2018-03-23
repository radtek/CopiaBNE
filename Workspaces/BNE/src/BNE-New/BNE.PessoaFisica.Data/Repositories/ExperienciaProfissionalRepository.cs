using System.Data.Entity;
using BNE.PessoaFisica.Domain.Model;
using BNE.PessoaFisica.Domain.Repositories;
using CrossCutting.Infrastructure.Repository;

namespace BNE.PessoaFisica.Data.Repositories
{
    public class ExperienciaProfissionalRepository : BaseRepository<ExperienciaProfissional, DbContext>, IExperienciaProfissionalRepository
    {
        public ExperienciaProfissionalRepository(DbContext dataContext) : base(dataContext)
        {
        }
    }
}