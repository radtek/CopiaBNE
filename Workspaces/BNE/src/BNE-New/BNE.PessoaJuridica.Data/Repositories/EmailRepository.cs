using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using BNE.PessoaJuridica.Domain.Model;
using BNE.PessoaJuridica.Domain.Repositories;
using CrossCutting.Infrastructure.Repository;

namespace BNE.PessoaJuridica.Data.Repositories
{
    public class EmailRepository : BaseRepository<Email, DbContext>, IEmailRepository
    {
        public EmailRepository(DbContext dataContext) : base(dataContext)
        {
        }

        public Email Get(Expression<Func<Email, bool>> where)
        {
            return DbSet.FirstOrDefault(where);
        }
    }
}