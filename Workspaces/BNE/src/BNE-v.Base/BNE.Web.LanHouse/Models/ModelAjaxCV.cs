using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BNE.Web.LanHouse.Models
{
    public class ModelAjaxCV
    {
        // segunda tela
        public string NomeCompleto { get; set; }
        public int Sexo { get; set; }
        public string DDD { get; set; }
        public string NumCelular { get; set; }

        // terceira tela
        public string Cpf { get; set; }
        public string DataNasc { get; set; }
        public string Email { get; set; }
        public string Cargo { get; set; }
        public string Salario { get; set; }

        // quinta tela
        public int Escolaridade { get; set; }

        public string InstituicaoEnsino { get; set; }
        public string NomeCurso { get; set; }
        public int IdCidadeEnsino { get; set; }
        public string CidadeEnsino { get; set; }
        public string PeriodoEnsino { get; set; }
        public int SituacaoEnsino { get; set; }
        public string AnoConclusaoEnsino { get; set; }

        public string NomeEmpresa1 { get; set; }
        public int AreaEmpresa1 { get; set; }
        public string DataAdmissao1 { get; set; }
        public string DataDemissao1 { get; set; }
        public string FuncaoExercida1 { get; set; }
        public string Atribuicoes1 { get; set; }
        public string UltimoSalario { get; set; }

        public string NomeEmpresa2 { get; set; }
        public int AreaEmpresa2 { get; set; }
        public string DataAdmissao2 { get; set; }
        public string DataDemissao2 { get; set; }
        public string FuncaoExercida2 { get; set; }
        public string Atribuicoes2 { get; set; }

        public string NomeEmpresa3 { get; set; }
        public int AreaEmpresa3 { get; set; }
        public string DataAdmissao3 { get; set; }
        public string DataDemissao3 { get; set; }
        public string FuncaoExercida3 { get; set; }
        public string Atribuicoes3 { get; set; }

        // sexta tela
        public int EstadoCivil { get; set; }
        public string Cep { get; set; }
        public string TelefoneRecado { get; set; }
        public string FalarCom { get; set; }
    }
}