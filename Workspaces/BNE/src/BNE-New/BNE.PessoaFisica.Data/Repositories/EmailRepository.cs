using System.Data.Entity;
using BNE.PessoaFisica.Domain.Model;
using BNE.PessoaFisica.Domain.Repositories;
using CrossCutting.Infrastructure.Repository;

namespace BNE.PessoaFisica.Data.Repositories
{
    public class EmailRepository : BaseRepository<Email, DbContext>, IEmailRepository
    {
        public EmailRepository(DbContext dataContext) : base(dataContext)
        {
        }
    }
}