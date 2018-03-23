using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using BNE.PessoaJuridica.Domain.Model;
using BNE.PessoaJuridica.Domain.Repositories;
using CrossCutting.Infrastructure.Repository;

namespace BNE.PessoaJuridica.Data.Repositories
{
    public class CNAERepository : BaseRepository<CNAE, DbContext>, ICNAERepository
    {
        public CNAERepository(DbContext dataContext) : base(dataContext)
        {
        }

        public CNAE Get(Expression<Func<CNAE, bool>> where)
        {
            return DbSet.FirstOrDefault(where);
        }
    }
}