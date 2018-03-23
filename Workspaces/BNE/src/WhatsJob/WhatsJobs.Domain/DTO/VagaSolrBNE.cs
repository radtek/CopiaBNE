using SolrNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsJob.Domain.DTO
{
    public class VagaSolrBNE
    {
        [SolrUniqueKey("id")]
        public string ID { get; set; }

        [SolrField("Des_Funcao")]
        public string Funcao { get; set; }

        [SolrField("Des_Area_BNE")]
        public string Area { get; set; }

        [SolrField("Nme_Cidade")]
        public string Cidade { get; set; }

        [SolrField("Sig_Estado")]
        public string UF { get; set; }
    }
}
