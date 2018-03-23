using System.Data.Entity;
using BNE.PessoaFisica.Domain.Model;
using BNE.PessoaFisica.Domain.Repositories;
using CrossCutting.Infrastructure.Repository;

namespace BNE.PessoaFisica.Data.Repositories
{
    public class FormacaoRepository : BaseRepository<Formacao, DbContext>, IFormacaoRepository
    {
        public FormacaoRepository(DbContext dataContext) : base(dataContext)
        {
        }
    }
}