namespace BNE.PessoaFisica.Domain.Command
{
    public class GetCursoCommand
    {
        public string Query { get; set; }
        public int Limit { get; set; }
    }
}