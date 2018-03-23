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
    public class InstituicaoEnsinoRepository : BaseRepository<InstituicaoEnsino, DbContext>, IInstituicaoEnsinoRepository
    {
        public InstituicaoEnsinoRepository(DbContext dataContext) : base(dataContext)
        {
        }

        public IEnumerable<InstituicaoEnsino> GetList(GetInstituicaoEnsinoCommand command)
        {
            return GetMany(InstituicaoEnsinoSpecs.GetList(command.Query)).OrderBy(n => n.Nome).Distinct().Take(command.Limit);
        }

        public InstituicaoEnsino GetByNome(string nome)
        {
            return Get(p => p.Nome.ToLower() == nome.ToLower());
        }

        public InstituicaoEnsino Get(Expression<Func<InstituicaoEnsino, bool>> where)
        {
            return DbSet.Where(where).FirstOrDefault();
        }

        public virtual IQueryable<InstituicaoEnsino> GetMany(Expression<Func<InstituicaoEnsino, bool>> where, params Expression<Func<InstituicaoEnsino, object>>[] includes)
        {
            var query = DbSet.Where(where).AsQueryable();
            if (includes != null)
                query = includes.Aggregate(query, (current, include) => current.Include(include));

            return query;
        }
    }
}