using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AdminLTE_Application.Models
{
    public class EsforcoVenda
    {

        public decimal? valorVenda { get; set; }

        public int? quantidadevenda { get; set; }

        public string Vendedor { get; set; }

        public string Num_CPF { get; set; }

        public int? Quantidade_Negociacao { get; set; }

        public int? Quantidade_Atendimento { get; set; }

        public int? QtdPago { get; set; }

        public decimal? TotalPago { get; set; }
        
    }
}