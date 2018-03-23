using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BNE.BLL.Custom.Solr
{
    public class ResultadoBuscaVagaSolr
    {
        public ResponseHeader responseHeader { get; set; }
        public Response response { get; set; }
        public Facet facet_counts { get; set; }
        public Stats stats { get; set; }
    }

    public class ResponseHeader
    {
        public int status { get; set; }
        public int QTime { get; set; }
        [JsonProperty(PropertyName = "params")]
        public Parameters param { get; set; }
    }

    public class Parameters
    {
        public string q { get; set; }
        public string start { get; set; }
        public string rows { get; set; }
    }

    public class Response
    {
        public int numFound { get; set; }
        public int start { get; set; }
        public decimal maxScore { get; set; }
        public List<dynamic> docs { get; set; }
        public Facet facet_counts { get; set; }
    }


    public class Stats
    {
        public StatsFields stats_fields { get; set; }
    }
    public class StatsFields
    {
        public VlrPretensaoSalarial Vlr_Pretensao_Salarial { get; set; }
    }
    public class VlrPretensaoSalarial
    {
        public decimal mean { get; set; }
    }

    public class Facet
    {
        public Dictionary<String, List<object>> facet_fields { get; set; }
        public Dictionary<String, List<Pivot>> facet_pivot { get; set; }
    }

    public class Pivot
    {
        public string field { get; set; }
        public string value { get; set; }
        public int count { get; set; }
        public List<Pivot> pivot { get; set; }
    }
}
