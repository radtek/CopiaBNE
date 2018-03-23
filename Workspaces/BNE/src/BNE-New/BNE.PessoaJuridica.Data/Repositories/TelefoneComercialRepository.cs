using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using BNE.PessoaJuridica.Domain.Model;
using BNE.PessoaJuridica.Domain.Repositories;
using CrossCutting.Infrastructure.Repository;

namespace BNE.PessoaJuridica.Data.Repositories
{
    public class TelefoneComercialRepository : BaseRepository<TelefoneComercial, DbContext>, ITelefoneComercialRepository
    {
        public TelefoneComercialRepository(DbContext dataContext) : base(dataContext)
        {
        }

        public TelefoneComercial Get(Expression<Func<TelefoneComercial, bool>> where)
        {
            return DbSet.FirstOrDefault(where);
        }
    }
}