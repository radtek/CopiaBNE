using System.Collections.Generic;
using SharedKernel.Repositories.Contracts;
using BNE.PessoaFisica.Domain.Command;

namespace BNE.PessoaFisica.Domain.Repositories
{
    public interface ICursoRepository : IBaseRepository<Model.Curso>
    {
        IEnumerable<Model.Curso> GetList(GetCursoCommand command);
        Model.Curso GetByDescricao(string descricao);
    }
}