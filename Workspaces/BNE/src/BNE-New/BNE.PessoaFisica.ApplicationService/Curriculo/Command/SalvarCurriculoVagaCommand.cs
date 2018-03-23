using System;
using System.Collections.Generic;

namespace BNE.PessoaFisica.ApplicationService.Curriculo.Command
{
    public class SalvarCurriculoVagaCommand
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
        public int IdVagaPergunta { get; set; }
        public string DescricaoVagaPergunta { get; set; }
        public bool FlagResposta { get; set; }//flag resposta da tb vaga_Pergunta
        public int TipoResposta { get; set; }
        public string Resposta { get; set; }//resposta tb vaga_candidato_resposta
        public bool? FlgRespostaPergunta { get; set; }//resposta tb vaga_candidato_resposta
    }

}