namespace BNE.Dashboard.Entities
{
    public class Enumerators
    {

        public enum ServiceType
        {
            WindowsService = 1,
            MessageQueue = 2
        }

        public enum WindowsServiceName
        {
            VerificarPlanoNaoEncerradoPessoaFisica = 1,
            VerificarPlanoNaoEncerradoPessoaJuridica = 2,
            VerificarQuantidadeCurriculoVIPComPlanoEncerrado = 3,
            VerificarQuantidadeNotaAntecipadaNaoEnviada = 4
        }
    }
}
