using SolrNet.Attributes;

namespace BNE.Web.Services.Solr.Models
{

    public class Funcao
    {

        [SolrUniqueKey("Id")]
        public int IdFuncao { get; set; }

        [SolrField("name")]
        public string NomeFuncao { get; set; }

        [SolrField("score")]
        public double? Score { get; set; }

    }
}