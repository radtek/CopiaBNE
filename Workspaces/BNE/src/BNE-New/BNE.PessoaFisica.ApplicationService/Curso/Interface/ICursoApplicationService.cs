using System.Collections.Generic;
using BNE.PessoaFisica.ApplicationService.Curso.Command;

namespace BNE.PessoaFisica.ApplicationService.Curso.Interface
{
    public interface ICursoApplicationService
    {
        IEnumerable<Model.CursoResponse> GetList(GetCursoCommand command);
    }
}