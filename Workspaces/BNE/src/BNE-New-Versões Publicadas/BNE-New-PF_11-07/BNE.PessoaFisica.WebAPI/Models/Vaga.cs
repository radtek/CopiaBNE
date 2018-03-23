using System;
using System.Collections.Generic;

namespace BNE.PessoaFisica.WebAPI.Models
{
    public class Vaga
    {
        public string Funcao { get; set; }
        public int? IdFuncao { get; set; }
        public int? IdCidade { get; set; }
        public int IdVaga { get; set; }

        public string SalarioDe { get; set; }
        public string SalarioAte { get; set; }
        public string Cidade { get; set; }
        public string UF { get; set; }

        public string CodigoVaga { get; set; }
        public string Atribuicoes { get; set; }
        public DateTime DataAnuncio { get; set; }
        public List<Pergunta> Perguntas { get; set; }
    }
    public class Pergunta
    {
        public int idVagaPergunta { get; set; }
        public string descricaoVagaPergunta { get; set; }
        public bool flagResposta { get; set; }//flag resposta da tb vaga_Pergunta
        public int tipoResposta { get; set; }
        public string resposta { get; set; }//resposta tb vaga_candidato_resposta
        public bool? flgRespostaPergunta { get; set; }//resposta tb vaga_candidato_resposta

    }

}