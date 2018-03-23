using System.Collections.Generic;
using System.Threading.Tasks;

namespace BNE.PessoaFisica.Domain.Services.SOLR
{
    public interface ISOLRService
    {
        Task<List<Vaga>> GetNavigation(int idVaga);
        Task<List<Vaga>> GetNavigation(int idVaga, int idPesquisaVaga, int jobIndex);
    }
}
