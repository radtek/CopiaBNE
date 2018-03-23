namespace BNE.Web.Vagas.Models
{
    public class VagaCompartilhamento
    {
        public int Identificador { get; set; }
        public string Funcao { get; set; }
        public string Cidade { get; set; }
        public decimal? SalarioInicial { get; set; }
        public decimal? SalarioFinal { get; set; }
        public string URLVaga { get; set; }
        public string URLIconeFacebook { get; set; }
    }
}