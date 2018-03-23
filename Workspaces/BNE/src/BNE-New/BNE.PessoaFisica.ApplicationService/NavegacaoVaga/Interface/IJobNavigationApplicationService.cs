using System.Threading.Tasks;
using BNE.PessoaFisica.ApplicationService.NavegacaoVaga.Command;
using BNE.PessoaFisica.ApplicationService.NavegacaoVaga.Model;

namespace BNE.PessoaFisica.ApplicationService.NavegacaoVaga.Interface
{
    public interface IJobNavigationApplicationService
    {
        Task<JobNavigationResponse> GetNavigation(JobNavigationCommand command);
    }
}