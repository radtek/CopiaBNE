using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using BNE.PessoaFisica.Domain.Command;
using BNE.PessoaFisica.Domain.Model;
using BNE.PessoaFisica.Domain.Repositories;
using BNE.PessoaFisica.Domain.Specs;
using CrossCutting.Infrastructure.Repository;

namespace BNE.PessoaFisica.Data.Repositories
{
    public class CursoRepository : BaseRepository<Curso, DbContext>, ICursoRepository
    {
        public CursoRepository(DbContext dataContext) : base(dataContext)
        {
        }

        public IEnumerable<Curso> GetList(GetCursoCommand command)
        {
            return GetMany(CursoSpecs.GetList(command.Query)).OrderBy(n => n.Descricao).Distinct().Take(command.Limit);
        }

        public Curso GetByDescricao(string descricao)
        {
            return Get(p => p.Descricao.ToLower() == descricao.ToLower());
        }

        public Curso Get(Expression<Func<Curso, bool>> where)
        {
            return DbSet.Where(where).FirstOrDefault();
        }

        public virtual IQueryable<Curso> GetMany(Expression<Func<Curso, bool>> where, params Expression<Func<Curso, object>>[] includes)
        {
            var query = DbSet.Where(where).AsQueryable();
            if (includes != null)
                query = includes.Aggregate(query, (current, include) => current.Include(include));

            return query;
        }
    }
}