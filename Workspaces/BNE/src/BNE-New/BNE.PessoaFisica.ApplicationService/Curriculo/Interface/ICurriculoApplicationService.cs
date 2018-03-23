using BNE.PessoaFisica.ApplicationService.Curriculo.Command;
using BNE.PessoaFisica.ApplicationService.Curriculo.Model;

namespace BNE.PessoaFisica.ApplicationService.Curriculo.Interface
{
    public interface ICurriculoApplicationService
    {
        InformacaoCurriculoResponse CarregarInformacoesCurriculo(RecuperarInformacaoCurriculoCommand command);
        CandidaturaDegustacao CarregarCadidaturasDegustacao(RecuperarCandidaturaDegustacaoCommand command);
        CandidaturaResponse CandidatarCurriculo(SalvarCurriculoCommand command);
        ExperienciaProfissionalResponse SalvarExperienciaProfissional(SalvarExperienciaProfissionalCommand command);
        FormacaoResponse SalvarFormacao(SalvarFormacaoCommand command);
        CadastroCurriculoResponse Cadastrar(SalvarCurriculoCommand command);
    }
}
