namespace BNE.PessoaFisica.ApplicationService.VagaPergunta.Model
{
    public class VagaPerguntaResponse
    {
        public int IdVagaPergunta { get; set; }
        public string DescricaoVagaPergunta { get; set; }
        public bool FlagResposta { get; set; }
        public int TipoResposta { get; set; }
    }
}