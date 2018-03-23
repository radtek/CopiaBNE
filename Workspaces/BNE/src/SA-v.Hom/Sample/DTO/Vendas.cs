using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AdminLTE_Application.Models
{
    public class Vendas
    {
        
        public string Data { get; set; }

        public int MinhasVendas { get; set; }

        public int Equipe { get; set; }
    }
}