using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminLTE_Application.Models
{
    public class EmpresasRamo
    {

        public string CNPJ { get; set; }
        public string Empresa { get; set; }
        public int Vagas { get; set; }
        public int Qtd_Visuaizacao_CV { get; set; }
        public string Des_Plano { get; set; }
    }
}