using System.Collections.Generic;
namespace BNE.Web.Vagas.Models
{
    public class VagaCandidatura
    {
        public int IdentificadorVaga { get; set; }
        public bool Candidatou { get; set; }
        public string URL { get; set; }
        public int TotalCandRealizadas { get; set; }
        public SucessoCandidatura Sucesso { get; set; }
        public DegustacaoCandidatura Degustacao { get; set; }
        public PerguntasCandidatura Perguntas { get; set; }
        public int idPerguntaExibir { get; set; }
        public bool FlgInativa { get; set; }
        public bool FlgVagaArquivada { get; set; }
        public PremiumCandidatura Premium { get; set;}
        public bool FlgCandidataOportunidade { get; set; }
        public bool IndicouTresAmigos { get; set; }
        public List<PessoaIndicada> PessoasIndicadas { get; set; }
        public bool IndicadoPeloBNE { get; set; }

        public class SucessoCandidatura
        {
            public string NomeCandidato { get; set; }
            public string Protocolo { get; set; }
        }

        public class DegustacaoCandidatura
        {
            public int QuantidadeCandidaturaRestante { get; set; }
            public string DescricaoCandidaturaRestante { get; set; }
          
        }

        public class PremiumCandidatura
        {
            public string PrecoCandidatura { get; set; }
            public string PrecoVip { get; set; }
            public bool premium { get; set; }
            public int idVaga { get; set; }
        }
    }

}