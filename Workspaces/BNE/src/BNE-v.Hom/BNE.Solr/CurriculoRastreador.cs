using System;
using System.Collections.Generic;

namespace BNE.Solr
{
    public class CurriculoRastreador : SolrResponse<CurriculoRastreador.Response>
    {
        public class Response : Docs
        {
            public int Idf_Curriculo { get; set; }
            public string Nme_Pessoa { get; set; }
            public string Nme_Cidade { get; set; }
            public string Sig_Estado { get; set; }
            public DateTime Dta_Nascimento { get; set; }
            public List<string> Des_Funcao { get; set; }
            public List<short> Qtd_Experiencia { get; set; }
        }
    }
}
