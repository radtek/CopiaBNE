using System.Collections.Generic;

namespace BNE.PessoaFisica.Domain.Repositories
{
    public interface IVagaRepository
    {
        Model.Vaga RecuperarVaga(int idVaga);
        IEnumerable<string> GetLinksPaginasSemelhantes(string funcao, string cidade, string siglaEstado, string areaBNE);
    }
}
