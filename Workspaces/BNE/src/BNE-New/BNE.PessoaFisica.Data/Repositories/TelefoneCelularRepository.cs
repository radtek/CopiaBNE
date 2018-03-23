using System;
using System.Data.Entity;
using System.Linq;
using BNE.PessoaFisica.Domain.Model;
using BNE.PessoaFisica.Domain.Repositories;
using CrossCutting.Infrastructure.Repository;

namespace BNE.PessoaFisica.Data.Repositories
{
    public class TelefoneCelularRepository : BaseRepository<TelefoneCelular, DbContext>, ITelefoneCelularRepository
    {
        public TelefoneCelularRepository(DbContext dataContext) : base(dataContext)
        {
        }

        public TelefoneCelular Get(Func<TelefoneCelular, bool> where)
        {
            return DbSet.Where(where).FirstOrDefault();
        }
    }
}