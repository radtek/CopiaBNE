using SolrNet.Attributes;

namespace BNE.Web.Services.Solr.Models
{
    public class Cidade
    {

        [SolrUniqueKey("Idf_Cidade")]
        public int IdCidade { get; set; }

        [SolrField("Nme_Cidade")]
        public string NomeCidade { get; set; }

        [SolrField("Sig_Estado")]
        public string SiglaEstado { get; set; }

        [SolrField("Nme_Estado")]
        public string NomeEstado { get; set; }

    }
}