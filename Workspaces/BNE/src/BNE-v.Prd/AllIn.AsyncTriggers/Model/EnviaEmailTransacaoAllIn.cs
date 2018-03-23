using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AllInTriggers.Model
{
    public class EnviaEmailTransacaoAllIn
    {
        public string Assunto { get; set; }

        public string[] Campos { get; set; }

        public DateTime? DataHoraEnvio { get; set; }

        public string EmailEnvio { get; set; }
        public string EmailRemente { get; set; }

        public string EmailResposta { get; set; }

        public string HtmAllInlId { get; set; }
        public string NomeRemente { get; set; }
        public string[] Valores { get; set; }
    }
}
