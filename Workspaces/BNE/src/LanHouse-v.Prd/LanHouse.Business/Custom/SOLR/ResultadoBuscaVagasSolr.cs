using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanHouse.Business.Custom.SOLR
{
    public class ResultadoBuscaVagasSolr
    {
        public ResponseHeader responseHeader { get; set; }
        public Response response { get; set; }
        public Dictionary<string, Highlight> highlighting { get; set; }
        public Spellcheck spellcheck { get; set; }
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
        public List<Doc> docs { get; set; }
    }

    public class Doc
    {
        public int id { get; set; }
        public DateTime Dta_Cadastro { get; set; }
        public DateTime Dta_Abertura { get; set; }
        public string Eml_Vaga { get; set; }
        public string Des_Atribuicoes { get; set; }
        public string Des_Requisito { get; set; }
        public string Des_Beneficio { get; set; }
        public string Des_Funcao { get; set; }
        public string Cod_Vaga { get; set; }
        public int Idf_Cidade { get; set; }
        public string Sig_Estado { get; set; }
        public string Nme_Cidade { get; set; }
        public int Qtd_Vaga { get; set; }
        public string Raz_Social { get; set; }
        public bool Flg_BNE_Recomenda { get; set; }
        public decimal Vlr_Salario_De { get; set; }
        public decimal Vlr_Salario_Para { get; set; }
        public int Idf_Funcao { get; set; }
        public DateTime Dta_Prazo { get; set; }
        public int? Num_Idade_Minima { get; set; }
        public int? Num_Idade_Maxima { get; set; }
        public bool Flg_Confidencial { get; set; }
    }

    public class Highlight
    {
        public List<string> Des_Atividade_Experiencia { get; set; }
        public List<string> text { get; set; }
    }

    public class Spellcheck
    {
        public List<object> suggestions { get; set; }
    }

    public class Suggestion
    {
        public int numFound { get; set; }
        public int startOffset { get; set; }
        public int endOffset { get; set; }
        List<String> suggestion { get; set; }
    }
}
