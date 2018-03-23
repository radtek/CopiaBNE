using System.Collections.Generic;

namespace BNE.Web.Vagas.Models
{
    public class VisualizarVaga
    {
        public Vaga Vaga { get; set; }
        public List<VagaSimilar> VagasSimilares { get; set; }
        public BNE.Web.Vagas.Code.Helpers.SEO.SEOLink LinkVagasFuncao { get; set; }
        public BNE.Web.Vagas.Code.Helpers.SEO.SEOLink LinkVagasCidade { get; set; }
        public BNE.Web.Vagas.Code.Helpers.SEO.SEOLink LinkVagasFuncaoCidade { get; set; }
        public BNE.Web.Vagas.Code.Helpers.SEO.SEOLink LinkVagasArea { get; set; }

        public Code.Helpers.SEO.PageAttributes PageAttributes { get; set; }
    }
}