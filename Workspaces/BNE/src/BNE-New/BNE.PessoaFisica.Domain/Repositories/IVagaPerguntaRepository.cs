using System.Collections.Generic;

namespace BNE.PessoaFisica.Domain.Repositories
{
    public interface IVagaPerguntaRepository
    {
        IEnumerable<Model.VagaPergunta> CarregarPergunta(int idVaga);
    }
}
