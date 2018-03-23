using BNE.Web.LanHouse.Code;
using FluentValidation.Attributes;

namespace BNE.Web.LanHouse.Models
{
    [Validator(typeof(FluentValidators.ModelAjaxQuintaTelaValidator))]
    public class ModelAjaxQuintaTela
    {
        [BindAlias("es")]
        public int Escolaridade { get; set; }
        [BindAlias("i")]
        public string InstituicaoEnsino { get; set; }
        [BindAlias("f")]
        public int? Fonte { get; set; } // o catalogo de instituicoes de ensino no BNE estao na tabela Fonte
        [BindAlias("nc")]
        public string NomeCurso { get; set; }
        [BindAlias("c")]
        public string Cidade { get; set; }
        [BindAlias("ic")]
        public int? IdCidade { get; set; }
        [BindAlias("p")]
        public int? Periodo { get; set; }
        [BindAlias("s")]
        public int? Situacao { get; set; }
        [BindAlias("a")]
        public int? AnoConclusao { get; set; }

        [BindAlias("ne1")]
        public string NomeEmpresa1 { get; set; }
        [BindAlias("ar1")]
        public int AreaEmpresa1 { get; set; }
        [BindAlias("da1")]
        public string DataAdmissao1 { get; set; }
        [BindAlias("dd1")]
        public string DataDemissao1 { get; set; }
        [BindAlias("f1")]
        public string FuncaoExercida1 { get; set; }
        [BindAlias("at1")]
        public string Atribuicoes1 { get; set; }

        [BindAlias("us")]
        public string UltimoSalario { get; set; }

        [BindAlias("ne2")]
        public string NomeEmpresa2 { get; set; }
        [BindAlias("ar2")]
        public int AreaEmpresa2 { get; set; }
        [BindAlias("da2")]
        public string DataAdmissao2 { get; set; }
        [BindAlias("dd2")]
        public string DataDemissao2 { get; set; }
        [BindAlias("f2")]
        public string FuncaoExercida2 { get; set; }
        [BindAlias("at2")]
        public string Atribuicoes2 { get; set; }

        [BindAlias("ne3")]
        public string NomeEmpresa3 { get; set; }
        [BindAlias("ar3")]
        public int AreaEmpresa3 { get; set; }
        [BindAlias("da3")]
        public string DataAdmissao3 { get; set; }
        [BindAlias("dd3")]
        public string DataDemissao3 { get; set; }
        [BindAlias("f3")]
        public string FuncaoExercida3 { get; set; }
        [BindAlias("at3")]
        public string Atribuicoes3 { get; set; }

    }
}