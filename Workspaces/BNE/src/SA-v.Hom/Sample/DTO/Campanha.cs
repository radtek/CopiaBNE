using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AdminLTE_Application.Models
{
    public class Campanha
    {
        public string Funcao { get; set; }
        public string Cidade { get; set; }
        public string UF { get; set; }
        public string Motivo { get; set; }
        public int QtdRetorno { get; set; }
        public int QtdCurriculo { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public string Dta_Cadastro { get; set; }
    }
}