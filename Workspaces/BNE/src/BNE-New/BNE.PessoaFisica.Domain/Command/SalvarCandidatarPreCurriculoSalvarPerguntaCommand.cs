namespace BNE.PessoaFisica.Domain.Command
{
    public class SalvarCandidatarPreCurriculoSalvarPerguntaCommand
    {
        public int IdVagaPergunta { get; set; }
        public bool? FlgRespostaPergunta { get; set; }
        public string Resposta { get; set; }
    }
}
