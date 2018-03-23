namespace BNE.PessoaFisica.ApplicationService.RamoAtividade.Command
{
    public class GetRamoAtividadeCommand
    {
        public string Query { get; set; }
        public int Limit { get; set; }
    }
}