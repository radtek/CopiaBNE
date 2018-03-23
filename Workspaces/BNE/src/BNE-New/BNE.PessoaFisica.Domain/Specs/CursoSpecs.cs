using System;
using System.Linq.Expressions;

namespace BNE.PessoaFisica.Domain.Specs
{
    public static class CursoSpecs
    {
        public static Expression<Func<Model.Curso, bool>> GetList(string nome)
        {
            return (x => x.Descricao.ToLower().StartsWith(nome) && x.Ativo);
        }
    }
}