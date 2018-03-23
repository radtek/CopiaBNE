namespace BNE.PessoaFisica.ApplicationService.Curriculo.Model
{
    public class CandidaturaDegustacao
    {
        public int QuantidadeCandidatura { get; set; }
        public bool CurriculoVIP { get; set; }
        public string URL { get; set; }

        public CandidaturaDegustacao()
        {
            QuantidadeCandidatura = 0;
            CurriculoVIP = false;
        }
    }
}