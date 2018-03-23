using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using BNE.PessoaJuridica.Domain.Model;
using BNE.PessoaJuridica.Domain.Repositories;
using CrossCutting.Infrastructure.Repository;

namespace BNE.PessoaJuridica.Data.Repositories
{
    public class UsuarioPessoaJuridicaRepository : BaseRepository<UsuarioPessoaJuridica, DbContext>, IUsuarioPessoaJuridicaRepository
    {
        public UsuarioPessoaJuridicaRepository(DbContext dataContext) : base(dataContext)
        {
        }

        public UsuarioPessoaJuridica Get(Expression<Func<UsuarioPessoaJuridica, bool>> where)
        {
            return DbSet.FirstOrDefault(where);
        }
    }
}