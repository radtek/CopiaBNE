using System.Collections.Generic;

namespace BNE.Web.Vagas.Models
{
    public class PerguntasCandidatura
    {
        public List<Pergunta> Perguntas { get; set; }

        public class Pergunta
        {
            public int Id { get; set; }
            public string Descricao { get; set; }
            public bool? Resposta { get; set; }
        }
    }
}