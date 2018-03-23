using System;
using System.Linq.Expressions;

namespace BNE.PessoaFisica.Domain.Specs
{
    public static class CodigoConfirmacaoEmailSpecs
    {
        public static Expression<Func<Model.CodigoConfirmacaoEmail, bool>> GetByCodigo(string codigo)
        {
            return x => x.Codigo == codigo;
        }
    }
}