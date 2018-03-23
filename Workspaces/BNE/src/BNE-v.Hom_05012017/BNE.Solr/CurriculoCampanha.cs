using System;

namespace BNE.Solr
{
    public class CurriculoCampanha : SolrResponse<CurriculoCampanha.Response>
    {
        public class Response : Docs
        {
            public int Idf_Curriculo { get; set; }
            public decimal Num_CPF { get; set; }
            public string Nme_Pessoa { get; set; }
            public string Eml_Pessoa { get; set; }
            public string Num_DDD_Celular { get; set; }
            public string Num_Celular { get; set; }
            public DateTime Dta_Nascimento { get; set; }
        }
    }
}
