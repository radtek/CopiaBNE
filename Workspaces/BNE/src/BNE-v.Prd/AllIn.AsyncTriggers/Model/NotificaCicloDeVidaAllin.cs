using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllInTriggers.Model
{
   

    public class NotificaCicloDeVidaAllIn
    {
        public NotificaCicloDeVidaAllIn()
        {
        }
        public bool AceitaRepeticao { get; set; }

        public KeyValuePair<string,string>[] CamposComValores { get; set; }

        public string EmailEnvio { get; set; }

        public string Evento { get; set; }
        public string IdentificadorAllIn { get; set; }

        public KeyValuePair<string, string>[] ListaParaAtualizacao { get; set; }
    }
}
