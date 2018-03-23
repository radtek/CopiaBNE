using BNE.PessoaFisica.ApplicationService.PessoFisica.Command;
using BNE.PessoaFisica.ApplicationService.PessoFisica.Model;

namespace BNE.PessoaFisica.ApplicationService.PessoFisica.Interface
{
    public interface IPessoaFisicaApplicationService
    {
        IndicarAmigosResponse IndicarAmigos(IndicarAmigosCommand objIndicacaoAmigo);
    }
}