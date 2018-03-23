using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AdminLTE_Application.Models
{
    public class Empresas
    {
        public string Empresa { get; set; }
        public string CNPJ { get; set; }
        public string Vendedor { get; set; }
        public string NomeFantasia { get; set; }        
        public string Dta_Inicio_Carteira { get; set; }
        public string Dta_Fim_Carteira { get; set; }
        public string Plano { get; set; }
        public string Dta_Inicio_Plano { get; set; }
        public string Dta_Fim_Plano { get; set; }
        public string Dta_Cadastro { get; set; }
        public string Des_Situacao_atendimento { get; set; }              
        public string Obs { get; set; }
    }
}