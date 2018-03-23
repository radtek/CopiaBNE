using System.Collections.Generic;
using SolrNet.Attributes;

namespace BNE.Web.Services.Solr.Models
{
    public class Bairro
    {

        [SolrUniqueKey("Idf_Bairro")]
        public int IdBairro { get; set; }

        [SolrField("Nme_Bairro")]
        public string NomeBairro { get; set; }

        [SolrField("Sig_Estado")]
        public string SiglaEstado { get; set; }

        [SolrField("Nme_Localidade")]
        public string NomeCidade { get; set; }

        [SolrField("Idf_Localidade")]
        public int IdCidade { get; set; }

        [SolrField("Cod_IBGE")]
        public string CodigoIBGE { get; set; }

        [SolrField("Nme_Bairro_Abreviado")]
        public string NomeAbreviado { get; set; }

        [SolrField("Nme_Bairro_Variacao")]
        public List<string> NomeVariacao { get; set; }

        [SolrField("Des_Zona")]
        public List<string> Zona { get; set; }

    }
}