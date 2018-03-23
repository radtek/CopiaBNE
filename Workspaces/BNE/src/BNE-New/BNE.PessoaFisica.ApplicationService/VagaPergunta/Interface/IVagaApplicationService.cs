using System.Collections.Generic;
using BNE.PessoaFisica.ApplicationService.VagaPergunta.Command;

namespace BNE.PessoaFisica.ApplicationService.VagaPergunta.Interface
{
    public interface IVagaPerguntaApplicationService
    {
        IEnumerable<Model.VagaPerguntaResponse> CarregarPergunta(GetByIdVagaCommand command);
    }
}
