using System.Collections.Generic;
using BNE.PessoaFisica.ApplicationService.InstituicaoEnsino.Command;

namespace BNE.PessoaFisica.ApplicationService.InstituicaoEnsino.Interface
{
    public interface IInstituicaoEnsinoApplicationService
    {
        IEnumerable<Model.InstituicaoEnsinoResponse> GetList(GetInstituicaoEnsinoCommand command);
    }
}