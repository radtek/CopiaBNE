namespace BNE.PessoaFisica.ApplicationService.NavegacaoVaga.Command
{
    public class JobNavigationCommand
    {
        public int SourceJob { get; set; }
        public int Job { get; set; }
        public int? JobSearch { get; set; }
        public int? JobIndex { get; set; }
    }
}