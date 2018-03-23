using System;

namespace BNE.Solr
{
    public class VagaCandidato : SolrResponse<VagaCandidato.Response>
    {
        public class Response : Docs
        {
            public int Idf_Curriculo { get; set; }
            public bool Perfil { get; set; }
            public DateTime Dta_Cadastro { get; set; }
            public DateTime Dta_Visualizacao { get; set; }
            public int Idf_Vaga { get; set; }
            public int Idf_Origem_Candidatura { get; set; }
            public string Idf_Vaga_Candidato { get; set; }
            public string Origem { get; set; }
            public long _version_ { get; set; }
            public bool Flg_Auto_Candidatura { get; set; }
            public bool[] Flg_Resposta { get; set; }
            public string[] Des_Resposta { get; set; }
            public bool[] Flg_Resposta_Esperada { get; set; }
            public string[] Des_Vaga_Pergunta { get; set; }



        }

    }
}
