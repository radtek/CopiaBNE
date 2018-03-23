using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using BNE.PessoaJuridica.Domain.Model;
using BNE.PessoaJuridica.Domain.Repositories;
using CrossCutting.Infrastructure.Repository;

namespace BNE.PessoaJuridica.Data.Repositories
{
    public class UsuarioRepository : BaseRepository<Usuario, DbContext>, IUsuarioRepository
    {
        public UsuarioRepository(DbContext dataContext) : base(dataContext)
        {
        }

        public Usuario Get(Expression<Func<Usuario, bool>> where)
        {
            return DbSet.FirstOrDefault(where);
        }
    }
}