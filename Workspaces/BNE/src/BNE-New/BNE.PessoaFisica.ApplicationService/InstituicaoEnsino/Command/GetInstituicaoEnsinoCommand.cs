namespace BNE.PessoaFisica.ApplicationService.InstituicaoEnsino.Command
{
    public class GetInstituicaoEnsinoCommand
    {
        public string Query { get; set; }
        public int Limit { get; set; }
    }
}