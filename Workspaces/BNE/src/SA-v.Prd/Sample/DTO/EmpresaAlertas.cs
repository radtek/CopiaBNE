using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AdminLTE_Application.Models
{
    public class EmpresaAlertas
    {
        public string Raz_Social { get; set; }
        public decimal Num_CNPJ { get; set; }
        public string Nme_Cidade { get; set; }
        public string Sig_Estado { get; set; }
        public string Des_CNAE_Sub_Classe { get; set; }
    }
}