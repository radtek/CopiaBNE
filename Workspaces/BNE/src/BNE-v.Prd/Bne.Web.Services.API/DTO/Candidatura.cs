
namespace Bne.Web.Services.API.DTO
{
    /// <summary>
    /// Classe que contém informações adicinais para a candidatura
    /// </summary>
    public class Candidatura
    {
        /// <summary>
        /// Respostas fornecidas pelo candidato
        /// </summary>
        public RespostaPergunta [] Respostas { get; set; }
    }
}