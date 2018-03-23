using System;
using System.Linq.Expressions;

namespace BNE.PessoaFisica.Domain.Specs
{
    public static class InstituicaoEnsinoSpecs
    {
        public static Expression<Func<Model.InstituicaoEnsino, bool>> GetList(string nome)
        {
            return (x => x.Nome.ToLower().StartsWith(nome) || x.Sigla.StartsWith(nome) && x.Ativo);
        }
    }
}