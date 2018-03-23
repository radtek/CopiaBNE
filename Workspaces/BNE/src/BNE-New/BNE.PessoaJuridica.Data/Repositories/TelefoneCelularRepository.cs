using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using BNE.PessoaJuridica.Domain.Model;
using BNE.PessoaJuridica.Domain.Repositories;
using CrossCutting.Infrastructure.Repository;

namespace BNE.PessoaJuridica.Data.Repositories
{
    public class TelefoneCelularRepository : BaseRepository<TelefoneCelular, DbContext>, ITelefoneCelularRepository
    {
        public TelefoneCelularRepository(DbContext dataContext) : base(dataContext)
        {
        }

        public TelefoneCelular Get(Expression<Func<TelefoneCelular, bool>> where)
        {
            return DbSet.FirstOrDefault(where);
        }
    }
}