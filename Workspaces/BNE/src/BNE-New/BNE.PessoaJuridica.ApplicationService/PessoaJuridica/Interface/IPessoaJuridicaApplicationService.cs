using BNE.PessoaJuridica.ApplicationService.PessoaJuridica.View;

namespace BNE.PessoaJuridica.ApplicationService.PessoaJuridica.Interface
{
    public interface IPessoaJuridicaApplicationService
    {
        CadastroEmpresaView CadastrarEmpresa(Command.CadastroEmpresa command);
    }
}