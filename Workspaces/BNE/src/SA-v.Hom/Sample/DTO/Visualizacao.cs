using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AdminLTE_Application.Models
{
    public class Visualizacao
    {
        
        public string Data { get; set; }

        public int VisualizacaoVIP { get; set; }

        public int VisualizacaoCompleta { get; set; }

        public int VisualizacaoNormal { get; set; }
    }
}