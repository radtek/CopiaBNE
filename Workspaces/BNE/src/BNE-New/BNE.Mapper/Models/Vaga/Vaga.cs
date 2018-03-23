using System;
using System.Collections.Generic;
using BNE.Mapper.Models.PessoaFisica;

namespace BNE.Mapper.Models.Vaga
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

        public string CodigoVaga { get; set; }
        public string Atribuicoes { get; set; }
        public string Beneficios { get; set; }
        public string Requisitos { get; set; }
        public DateTime DataAnuncio { get; set; }
        public bool FlgAuditada { get; set; }
        public bool FlgArquivada { get; set; }
        public bool FlgInativo { get; set; }
        public bool FlgConfidencial { get; set; }
        public string NomeEmpresa { get; set; }
        public string Descricao { get; set; }
        public string SalarioDescricao { get; set; }
        public string[] DescricaoTipoVinculo { get; set; }
        public string DescricaoAreaBNEPesquisa { get; set; }
        public bool? FlgDeficiencia { get; set; }
        public string DescricaoDeficiencia { get; set; }
        public int Idf_Deficiencia { get; set; }

        public bool eEstagio { get; set; }
        public bool eEfetivo { get; set; }
        public bool eAprendiz { get; set; }

        public bool FlgPremium { get; set; }
        //pegar o parametro do preço do plano quando a vaga é premium e jogar na modal
        public PlanoPremium PlanoPremium { get; set; }
        public List<Pergunta> Perguntas { get; set; }
        public string Bairro { get; set; }
    }

    public class Pergunta
    {
        public string descricaoVagaPergunta;
        public bool flagResposta;
        public int idVagaPergunta;
        public int tipoResposta;
    }
}