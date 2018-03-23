namespace BNE.PessoaFisica.Domain.Command
{
    public class GetInstituicaoEnsinoCommand
    {
        public string Query { get; set; }
        public int Limit { get; set; }
    }
}