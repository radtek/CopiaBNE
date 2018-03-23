namespace BNE.PessoaFisica.Domain.Model
{
    public class VagaPergunta
    {
        public int IdVagaPergunta { get; set; }
        public string DescricaoVagaPergunta { get; set; }
        public bool FlagResposta { get; set; }
        public int TipoResposta { get; set; }
    }
}
