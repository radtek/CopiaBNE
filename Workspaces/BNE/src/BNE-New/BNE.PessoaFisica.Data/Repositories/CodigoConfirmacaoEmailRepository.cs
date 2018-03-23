using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using BNE.PessoaFisica.Domain.Model;
using BNE.PessoaFisica.Domain.Repositories;
using BNE.PessoaFisica.Domain.Specs;
using CrossCutting.Infrastructure.Repository;

namespace BNE.PessoaFisica.Data.Repositories
{
    public class CodigoConfirmacaoEmailRepository : BaseRepository<CodigoConfirmacaoEmail, DbContext>, ICodigoConfirmacaoEmailRepository
    {
        public CodigoConfirmacaoEmailRepository(DbContext dataContext) : base(dataContext)
        {
        }

        public CodigoConfirmacaoEmail GetByCodigo(string codigo)
        {
            return Get(CodigoConfirmacaoEmailSpecs.GetByCodigo(codigo));
        }

        public CodigoConfirmacaoEmail Get(Expression<Func<CodigoConfirmacaoEmail, bool>> where)
        {
            return DbSet.Where(where).FirstOrDefault();
        }
    }
}