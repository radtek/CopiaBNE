using System;
using System.Collections.Generic;
using BNE.PessoaFisica.Web.Helpers.SEO;

namespace BNE.PessoaFisica.Web.Models
{
    public class Vaga
    {
        public string Funcao { get; set; }
        public int? IdFuncao { get; set; }
        public int? IdCidade { get; set; }
        public int IdVaga { get; set; }
        public int IdTipoVinculo { get; set; }

        public decimal SalarioDe { get; set; }
        public decimal SalarioAte { get; set; }
        public string Cidade { get; set; }
        public string UF { get; set; }
        public string Descricao { get; set; }

        public string CodigoVaga { get; set; }
        public string Atribuicoes { get; set; }
        public string Beneficios { get; set; }
        public string Requisitos { get; set; }

        public DateTime DataAnuncio { get; set; }

        public string NomeEmpresa { get; set; }
        public bool FlgAuditada { get; set; }
        public bool FlgArquivada { get; set; }
        public bool FlgInativo { get; set; }
        public bool FlgConfidencial { get; set; }

        public string faixaSalarial { get; set; }
        public decimal? faixaSalarialDe { get; set; }
        public decimal? faixaSalarialAte { get; set; }
        public int IdPesquisa { get; set; }
        public string urlPesquisa { get; set; }
        public string[] DescricaoTipoVinculo { get; set; }
        public string DescricaoAreaBNEPesquisa { get; set; }

        public bool FlagDeficiencia { get; set; }
        public string DescricaoDeficiencia { get; set; }
        public int Idf_Deficiencia { get; set; }

        public bool eEstagio { get; set; }
        public bool eEfetivo { get; set; }
        public bool eAprendiz { get; set; }

        public SEOLink LinkVagasFuncao { get; set; }
        public SEOLink LinkVagasCidade { get; set; }
        public SEOLink LinkVagasFuncaoCidade { get; set; }
        public SEOLink LinkVagasArea { get; set; }

        public bool FlgPremium { get; set; }
        public PlanoPremium PlanoPremium { get; set; }
        public List<Pergunta> Perguntas { get; set; }
        public string Bairro { get; set; }
        public IList<string> LinkPaginasSemelhantes { get; set; }
        public NavegacaoVaga Navegacao { get; set; }
    
    }
    
    public class PlanoPremium
    {
        public string PrecoCandidatura { get; set; }
        public string PrecoVip { get; set; }
    }

    public class Pergunta
    {
        public int idVagaPergunta { get; set; }
        public string descricaoVagaPergunta { get; set; }
        public bool flagResposta { get; set; } //flag resposta da tb vaga_Pergunta
        public int tipoResposta { get; set; }
        public string resposta { get; set; } //resposta tb vaga_candidato_resposta
        public bool? flgRespostaPergunta { get; set; } //resposta tb vaga_candidato_resposta
    }
}