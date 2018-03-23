using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminLTE_Application.Models
{
    public class Limites
    {
        public int QtdSmsDisponivel { get; set; }

        public int QtdVisualizacao { get; set; }

        public int QtdVisualizacaoUtilizado { get; set; }

        public int QtdUsuario { get; set; }

        public Boolean ParcelaAtraso { get; set; }

        public int QtdCampanhas { get; set; }
    }
}