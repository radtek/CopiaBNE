using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BNE.PessoaFisica.Domain.Command
{
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
