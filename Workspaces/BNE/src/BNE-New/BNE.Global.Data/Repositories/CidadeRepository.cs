using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using BNE.Global.Model;
using CrossCutting.Infrastructure.Repository;
using SharedKernel.Repositories.Contracts;

namespace BNE.Global.Data.Repositories
{
    public class CidadeRepository : BaseRepository<Cidade, DbContext>, ICidadeRepository
    {
        public CidadeRepository(DbContext dataContext) : base(dataContext)
        {
        }

        public Cidade GetByNomeUF(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return null;

            string siglaEstado;
            var nomeCidade = siglaEstado = string.Empty;

            #region Recuperando estado, se tiver

            const string pattern = @"([\w\s'-]+)[/]([a-zA-Z]{2})"; //Ex. Curitiba/Paraná
            var regex = new Regex(pattern);
            var match = regex.Match(query.Trim());
            if (match.Success)
            {
                nomeCidade = match.Groups[1].Value;
                siglaEstado = match.Groups[2].Value;
            }

            #endregion

            return Get(n => n.Nome == nomeCidade && n.Estado.UF == siglaEstado);
        }

        public Cidade Get(Expression<Func<Cidade, bool>> exp)
        {
            return DbSet.Where(exp).FirstOrDefault();
        }
    }

    public interface ICidadeRepository : IBaseRepository<Cidade>
    {
        Cidade GetByNomeUF(string query);
        Cidade Get(Expression<Func<Cidade, bool>> where);
    }
}