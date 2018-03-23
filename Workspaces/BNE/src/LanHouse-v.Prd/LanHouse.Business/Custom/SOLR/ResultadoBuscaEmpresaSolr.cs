using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanHouse.Business.Custom.SOLR
{
    public class ResultadoBuscaEmpresaSolr
    {
        public ResponseHeaderEmpresa responseHeader { get; set; }
        public ResponseEmpresa response { get; set; }
        public Dictionary<string, HighlightEmpresa> highlighting { get; set; }
        public SpellcheckEmpresa spellcheck { get; set; }
    }

    public class ResponseHeaderEmpresa
    {
        public int status { get; set; }
        public int QTime { get; set; }
        [JsonProperty(PropertyName = "params")]
        public ParametersEmpresa param { get; set; }
    }

    public class ParametersEmpresa
    {
        public string q { get; set; }
        public string start { get; set; }
        public string rows { get; set; }
    }

    public class ResponseEmpresa
    {
        public int numFound { get; set; }
        public int start { get; set; }
        public decimal maxScore { get; set; }
        public List<DocEmpresa> docs { get; set; }
    }

    public class DocEmpresa
    {
        public int id { get; set; }
        public string Nme_Fantasia { get; set; }
        public string Eml_Comercial { get; set; }
        public string Des_Carta_Apresentacao { get; set; }
    }

    public class HighlightEmpresa
    {
        public List<string> Des_Atividade_Experiencia { get; set; }
        public List<string> text { get; set; }
    }

    public class SpellcheckEmpresa
    {
        public List<object> suggestions { get; set; }
    }

    public class SuggestionEmpresa
    {
        public int numFound { get; set; }
        public int startOffset { get; set; }
        public int endOffset { get; set; }
        List<String> suggestion { get; set; }
    }
}
