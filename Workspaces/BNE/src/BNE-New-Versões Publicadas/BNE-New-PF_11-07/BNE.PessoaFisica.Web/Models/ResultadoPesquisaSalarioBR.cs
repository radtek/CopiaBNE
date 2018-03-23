using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BNE.PessoaFisica.Web.Models
{
    public class ResultadoPesquisaSalarioBR
    {
        public int IdFuncaoSalarioBR { get; set; }
        public String NomeFuncao { get; set; }
        public string DescricaoFuncao { get; set; }

        public DetalhesFuncao DetalhesFuncao { get; set; }
    }

    public class DetalhesFuncao
    {
        public string ObjetivosDoCargo { get; set; }

        public SalarioPequena SalarioPequena { get; set; }
        public SalarioGrande SalarioGrande { get; set; }

    }

    public class SalarioPequena
    {
        public decimal Trainee { get; set; }
        public decimal Junior { get; set; }
        public decimal Pleno { get; set; }
        public decimal Master { get; set; }

    }

    public class SalarioGrande
    {
        public decimal Master { get; set; }
    }
}