
namespace BNE.PessoaFisica.Domain.Repositories
{
    public interface IDadosEmpresaRepository 
    {
        Model.DadosEmpresa RecuperarDados(int? idCurriculo, int idVaga);
    }
}