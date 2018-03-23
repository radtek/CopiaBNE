using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AdminLTE_Application.Models
{
    public class RankingVendas
    {
        
        public string vendedor { get; set; }

        public decimal valorTotal  { get; set; }

        public int quantidadeTotal { get; set; }
    }
}