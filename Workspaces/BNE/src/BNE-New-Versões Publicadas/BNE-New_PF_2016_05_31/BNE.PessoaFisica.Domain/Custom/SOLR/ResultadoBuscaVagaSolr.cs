using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BNE.PessoaFisica.Domain.Custom.SOLR
{
    public class ResultadoBuscaVagaSolr
    {
        public ResponseHeader responseHeader { get; set; }
        public Response response { get; set; }
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
        public List<Doc> docs { get; set; }
    }

    public class Doc
    {
        public int id { get; set; }
        public DateTime Dta_Abertura { get; set; }
        public string Des_Atribuicoes { get; set; }
        public string Des_Requisito { get; set; }
        public string Des_Beneficio { get; set; }


        public string Des_Funcao { get; set; }
        public string Cod_Vaga { get; set; }
        public int Idf_Cidade { get; set; }
        public string Sig_Estado { get; set; }
        public string Nme_Cidade { get; set; }
        public int Qtd_Vaga { get; set; }
        public bool Flg_BNE_Recomenda { get; set; }
        public bool Flg_Auditada { get; set; }
        public bool Flg_Vaga_Arquivada { get; set; }
        public bool Flg_Inativo { get; set; }

        public decimal Vlr_Salario_De { get; set; }
        public decimal Vlr_Salario_Para { get; set; }
        public int Idf_Funcao { get; set; }
        public string Nme_Empresa { get; set; }
        public string[] Des_Tipo_Vinculo { get; set; }
        public int[] Idf_Tipo_Vinculo { get; set; }

        public string Des_Area_BNE { get; set; }

        public bool Flg_Deficiencia { get; set; }
        public string Des_Deficiencia { get; set;}
        public int Idf_Deficiencia { get; set; }

        public bool Flg_Vaga_Premium { get; set; }
    }
}