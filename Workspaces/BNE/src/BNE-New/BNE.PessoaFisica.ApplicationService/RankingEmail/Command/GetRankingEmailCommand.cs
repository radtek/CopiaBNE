namespace BNE.PessoaFisica.ApplicationService.RankingEmail.Command
{
    public class GetRankingEmailCommand
    {
        public string Query { get; set; }
        public int Limit { get; set; }
    }
}