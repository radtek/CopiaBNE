using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AdminLTE_Application.Models
{
    public class NumeroCvsCidade
    {
        
        
        public int Idf_Cidade { get; set; }

        public string Nme_Cidade { get; set; }

        public string UF { get; set; }
        
        public int Total { get; set; }
    }
}