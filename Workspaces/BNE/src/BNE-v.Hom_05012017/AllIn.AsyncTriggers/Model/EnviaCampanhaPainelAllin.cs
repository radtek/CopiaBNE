using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllInTriggers.Model
{
    public class EnviaCampanhaPainelAllIn
    {
        public string Campanha { get; set; }
        public string Assunto { get; set; }
        public string NomeRemente { get; set; }
        public string EmailRemente { get; set; }
        public string EmailResposta { get; set; }

        public DateTime? DataHoraEnvio { get; set; }
        public string Html { get; set; }

        public string Filtro { get; set; }
        public bool UtilizarAnalytics { get; set; }
        public string Categoria { get; set; }
        public string NomeLista { get; set; }
    }
}
