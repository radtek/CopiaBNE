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
            VerificarQuantidadeNotaAntecipadaNaoEnviada = 4,
            CidadeDoCurriculoNaoSincronizado = 6,
            PerfilCandidatoComFilial = 7,
            PerfilEmpresaSemFilial = 8,
            PerfilCandidadoComDoisPerfis = 9,
            PerfilEmpresaComDoisPerfis = 10,
            PerfilErradoNoPlanoAdquirido = 11,
            VerificarQuantidadePlanoLiberadoCandidatoSemVIP = 13
        }
    }
}
