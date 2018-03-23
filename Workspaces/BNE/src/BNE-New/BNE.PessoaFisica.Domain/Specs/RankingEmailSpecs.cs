using System;
using System.Linq.Expressions;
using BNE.Global.Model;

namespace BNE.PessoaFisica.Domain.Specs
{
    public class RankingEmailSpecs
    {
        public static Expression<Func<RankingEmail, bool>> GetList(string query, string sulfixo)
        {
            return x => x.DescricaoEmail.ToLower().StartsWith(sulfixo);
        }
    }
}
