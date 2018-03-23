using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AdminLTE_Application.Models
{
    public class HistoricoAtendente
    {
        
        public string Vendedor { get; set; }

        public string Inicio { get; set; }

        public string Fim { get; set; }

        public int QtdAtendimentos { get; set; }

        public int QtdNegociacao { get; set; }

        public int QtdVenda { get; set; }

        public string Obs { get; set; }

        
    }
}