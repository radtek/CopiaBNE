using SolrNet.Attributes;

namespace BNE.Solr.Test
{
    public class Vaga
    {

        [SolrField("Idf_Curriculo")]
        public int IdCurriculo { get; set; }
        
    }
}
